Imports System.Security.Cryptography
Imports System.IO
Imports System.Security


Module mdlTests


    Sub GetHashesOfFilesInFolder(strPath As String)


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
                IniWrite(strPath & "\hashes.cfg", Strings.Replace(Strings.Replace(Path.GetDirectoryName(f), strPath & "\", ""), "\", "/"), Path.GetFileName(f), Hash_generator("sha256", f))
            Next

25:

            ' Loop through all subdirectories and add them to the stack.
            Dim directoryName As String
            For Each directoryName In Directory.GetDirectories(dir)
                stack.Push(directoryName)
            Next

        Loop



    End Sub

    ' Function to obtain the desired hash of a file
    Function Hash_generator(ByVal hash_type As String, ByVal file_name As String)

        ' Declaring the variable : hash
        Dim hash
        If hash_type.ToLower = "md5" Then
            ' Initializes a md5 hash object
            hash = MD5.Create
        ElseIf hash_type.ToLower = "sha1" Then
            ' Initializes a SHA-1 hash object
            hash = SHA1.Create()
        ElseIf hash_type.ToLower = "sha256" Then
            ' Initializes a SHA-256 hash object
            hash = SHA256.Create()
        Else
            MsgBox("Unknown type of hash : " & hash_type, MsgBoxStyle.Critical)
            Return False
        End If

        ' Declaring a variable to be an array of bytes
        Dim hashValue() As Byte

        ' Creating e a FileStream for the file passed as a parameter
        Dim fileStream As FileStream = File.OpenRead(file_name)
        ' Positioning the cursor at the beginning of stream
        fileStream.Position = 0
        ' Calculating the hash of the file
        hashValue = hash.ComputeHash(fileStream)
        ' The array of bytes is converted into hexadecimal before it can be read easily
        Dim hash_hex = PrintByteArray(hashValue)

        ' Closing the open file
        fileStream.Close()

        ' The hash is returned
        Return hash_hex

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
