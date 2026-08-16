Imports System.Security.Cryptography
Imports System.IO
Imports System.Security


Module MdlTests

    ''' <summary>
    ''' Recursively processes all files in a folder and writes the hash files out to a .INI file
    ''' </summary>
    ''' <param name="StrPath">Source Folder</param>
    ''' <param name="StrDestinationFileName">Destination file name</param>
    Sub GetHashesOfFilesInFolder(StrPath As String, StrDestinationFileName As String)

        Try

            ' Track how many files succeeded/failed, to report a summary at the end.
            ' A single unreadable file (e.g. locked, permission denied) should not abort hashing
            ' the rest of the tree; same reasoning as issue #44 for the main batch operations.
            Dim IntSucceeded As Integer = 0
            Dim LstFailed As New List(Of String)

            ' Accumulate "key=value" lines and write StrDestinationFileName once at the end, instead
            ' of calling IniWrite (-> the native WritePrivateProfileString) once per file. That native
            ' call rewrites the whole destination file on every invocation, which turns hashing a large
            ' folder (thousands of files, realistic for a full ZT1 asset scan) into thousands of
            ' separate full-file rewrites of the same output file.
            Dim LstHashLines As New List(Of String)

            BlnBatchOperationRunning = True
            Try

                For Each StrCurrentDirectory As String In MdlBatch.EnumerateDirectoriesRecursive(StrPath)

                    For Each StrCurrentFile As String In Directory.GetFiles(StrCurrentDirectory, "*")

                        Try
                            Dim ObjHash As String = MdlTests.GenerateHash("sha256", StrCurrentFile)
                            LstHashLines.Add(StrCurrentFile.Replace(StrPath & "\", "") & "=" & ObjHash)
                            IntSucceeded += 1

                        Catch exFile As Exception
                            MdlZTStudio.LogError("MdlTests", "GetHashesOfFilesInFolder", "Failed to hash file: " & StrCurrentFile, exFile)
                            LstFailed.Add(StrCurrentFile)
                        End Try

                    Next

                Next

            Finally
                BlnBatchOperationRunning = False
            End Try

            ' Write the whole [Hashes] section in one go. StrDestinationFileName is a dedicated
            ' output file for this action (e.g. hashes.cfg), not shared with settings.cfg, so there
            ' is no other content in it to preserve.
            Dim StrOutput As New Text.StringBuilder()
            StrOutput.AppendLine("[Hashes]")
            For Each StrHashLine As String In LstHashLines
                StrOutput.AppendLine(StrHashLine)
            Next
            File.WriteAllText(StrDestinationFileName, StrOutput.ToString())

            ' Report a summary of the batch run, rather than silently succeeding or hard-crashing.
            Dim StrSummary As String = IntSucceeded & " file(s) hashed successfully."
            If LstFailed.Count > 0 Then
                StrSummary = StrSummary & vbCrLf & LstFailed.Count & " file(s) failed:" & vbCrLf & String.Join(vbCrLf, LstFailed)
            End If
            MdlZTStudio.InfoBox("MdlTests", "GetHashesOfFilesInFolder", StrSummary)

        Catch ex As Exception
            ' Genuinely fatal: e.g. the target folder itself could not be scanned at all.
            MdlZTStudio.HandledError("MdlTests", "GetHashesOfFilesInFolder", "Unexpected error occurred.", True, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Function to obtain the desired hash of a file
    ''' </summary>
    ''' <param name="StrHashType">Hash type</param>
    ''' <param name="StrFileName">Source file name</param>
    ''' <returns></returns>
    Function GenerateHash(ByVal StrHashType As String, ByVal StrFileName As String)

        ' Typed as HashAlgorithm (the actual common base of MD5/SHA1/SHA256) rather than Object, so
        ' ComputeHash() below is a normal virtual call instead of a late-bound (reflection) dispatch -
        ' this runs once per file during a folder-wide hash scan.
        Dim HashGenerator As HashAlgorithm
        Select Case StrHashType
            Case "md5"
                HashGenerator = MD5.Create()

            Case "sha1"
                HashGenerator = SHA1.Create()

            Case "sha256"
                HashGenerator = SHA256.Create()

            Case Else

                MdlZTStudio.HandledError("MdlTests", "GenerateHash", "Unknown type of hash: " & StrHashType, False, Nothing)
                Return Nothing

        End Select

        ' Declaring a variable to be an array of bytes
        Dim HashValue() As Byte

        ' Creating a FileStream for the file passed as a parameter.
        ' Wrapped in a Using block so the file handle is always released, even if ComputeHash throws
        ' (e.g. the file gets deleted/locked by another process mid-read during a folder-wide scan).
        Using FileStream As FileStream = File.OpenRead(StrFileName)

            ' Positioning the cursor at the beginning of stream
            FileStream.Position = 0
            ' Calculating the hash of the file
            HashValue = HashGenerator.ComputeHash(FileStream)

        End Using

        ' The array of bytes is converted into hexadecimal before it can be read easily
        Dim ObjHash = PrintByteArray(HashValue)

        ' The hash is returned
        Return ObjHash

    End Function

    ''' <summary>
    ''' Traverse the array of bytes and converting each byte in hexadecimal
    ''' </summary>
    ''' <param name="array">Byte array</param>
    ''' <returns></returns>
    Public Function PrintByteArray(ByVal Array() As Byte)

        Dim hex_value As String = ""

        ' Traverse the array of bytes
        Dim I As Integer
        For I = 0 To Array.Length - 1

            ' Convert each byte in hexadecimal
            hex_value += Array(I).ToString("X2")

        Next I

        ' Return the string in lowercase
        Return hex_value.ToLower

    End Function





End Module
