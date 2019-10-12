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

        ' First create a recursive list.

        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim StackDirectories As New Stack(Of String)

        ' Add the initial directory
        StackDirectories.Push(StrPath)

10:

        ' Continue processing for each stacked directory
        Do While (StackDirectories.Count > 0)
            ' Get top directory string

15:
            Dim StrCurrentDirectory As String = StackDirectories.Pop

20:
            For Each StrCurrentFile As String In Directory.GetFiles(StrCurrentDirectory, "*")

                Dim ObjHash As String = MdlTests.GenerateHash("sha256", StrCurrentFile)
                IniWrite(StrDestinationFileName, "Hashes", StrCurrentFile.Replace(StrPath & "\", ""), ObjHash)
                'ObjHash.dispose()

            Next

29:

30:

            ' Loop through all subdirectories and add them to the stack.
            Dim StrSubDirectoryName As String
            For Each StrSubDirectoryName In Directory.GetDirectories(StrCurrentDirectory)
                StackDirectories.Push(StrSubDirectoryName)
            Next

        Loop



    End Sub

    ''' <summary>
    ''' Function to obtain the desired hash of a file
    ''' </summary>
    ''' <param name="StrHashType">Hash type</param>
    ''' <param name="StrFileName">Source file name</param>
    ''' <returns></returns>
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

                MdlZTStudio.HandledError("MdlTests", "GenerateHash", "Unknown type of hash: " & StrHashType, False, Nothing)
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
