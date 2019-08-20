Imports System.Security.Cryptography
Imports System.IO
Imports System.Security


Module MdlTests


    Sub GetHashesOfFilesInFolder(strPath As String)


        ' First we will create a recursive list.

        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim StackDirectories As New Stack(Of String)

        ' Add the initial directory
        StackDirectories.Push(strPath)

10:

        ' Continue processing for each stacked directory
        Do While (StackDirectories.Count > 0)
            ' Get top directory string

15:
            Dim StrCurrentDirectory As String = StackDirectories.Pop

20:
            For Each StrCurrentFile As String In Directory.GetFiles(StrCurrentDirectory, "*")


                Dim ObjHash As Object = MdlTests.GenerateHash("sha256", StrCurrentFile)
                StrCurrentFile = Strings.Replace(Strings.Replace(Path.GetDirectoryName(StrCurrentFile), strPath & "\", ""), "\", "/")
                IniWrite(strPath & "\hashes.cfg", StrCurrentFile, Path.GetFileName(StrCurrentFile), ObjHash)

                ObjHash.dispose()

            Next

30:

            ' Loop through all subdirectories and add them to the stack.
            Dim StrSubDirectoryName As String
            For Each StrSubDirectoryName In Directory.GetDirectories(Dir)
                StackDirectories.Push(StrSubDirectoryName)
            Next

        Loop



    End Sub

    ' Function to obtain the desired hash of a file
    Function GenerateHash(ByVal StrHashType As String, ByVal StrFileName As String)

        ' Declaring the variable : hash
        Dim HashGenerator As Object
        Select Case StrHashType
            Case "md5"
                HashGenerator = MD5.Create()

            Case "sha1"
                HashGenerator = SHA1.Create()

            Case "sha256"
                HashGenerator = SHA256.Create()

            Case Else
                MsgBox("Unknown type of hash: " & StrHashType, MsgBoxStyle.Critical, "Unknown hash type")
                Return Nothing

        End Select

        ' Declaring a variable to be an array of bytes
        Dim HashValue() As Byte

        ' Creating e a FileStream for the file passed as a parameter
        Dim FileStream As FileStream = File.OpenRead(StrFileName)
        ' Positioning the cursor at the beginning of stream
        FileStream.Position = 0
        ' Calculating the hash of the file
        HashValue = HashGenerator.ComputeHash(FileStream)
        ' The array of bytes is converted into hexadecimal before it can be read easily
        Dim ObjHash = PrintByteArray(HashValue)

        ' Closing the open file
        FileStream.Close()

        ' The hash is returned
        Return ObjHash

    End Function

    ' We traverse the array of bytes and converting each byte in hexadecimal
    Public Function PrintByteArray(ByVal array() As Byte)

        Dim hex_value As String = ""

        ' We traverse the array of bytes
        Dim I As Integer
        For i = 0 To array.Length - 1

            ' We convert each byte in hexadecimal
            hex_value += array(i).ToString("X2")

        Next i

        ' We return the string in lowercase
        Return hex_value.ToLower

    End Function





End Module
