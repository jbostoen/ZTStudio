Imports System.Security.Cryptography
Imports System.IO
Imports System.Security


Module MdlTests


    Sub Get_HashesOfFilesInFolder(strPath As String)


        ' First we will create a recursive list.

        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(strPath)

10:

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string

15:
            Dim dir As String = stack.Pop

20:
            For Each f As String In Directory.GetFiles(dir, "*")
                Debug.Print(f)
                Dim objHash As Object = MdlTests.Generate_Hash("sha256", f)
                IniWrite(strPath & "\hashes.cfg", Strings.Replace(Strings.Replace(Path.GetDirectoryName(f), strPath & "\", ""), "\", "/"), Path.GetFileName(f), objhash)

                objHash.dispose()

            Next

25:

30:

            ' Loop through all subdirectories and add them to the stack.
            Dim directoryName As String
            For Each directoryName In Directory.GetDirectories(dir)
                stack.Push(directoryName)
            Next

        Loop



    End Sub

    ' Function to obtain the desired hash of a file
    Function Generate_Hash(ByVal StrHashType As String, ByVal StrFileName As String)

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
        Dim i As Integer
        For i = 0 To array.Length - 1

            ' We convert each byte in hexadecimal
            hex_value += array(i).ToString("X2")

        Next i

        ' We return the string in lowercase
        Return hex_value.ToLower

    End Function





End Module
