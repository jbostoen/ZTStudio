
Imports System.IO

''' <summary>
''' Groups batch operations
''' </summary>
Module MdlBatch


    ''' <summary>
    ''' Attempts to create .ani file for each animation. Experimental.
    ''' </summary>
    ''' <param name="StrPath">Path to folder</param>
    Sub WriteAniFile(StrPath As String)


        If Cfg_export_ZT1_Ani = 0 Then

            MdlZTStudio.Trace("MdlBatch", "WriteAniFile", "Option to create .ani not enabled. Skipping main folder " & StrPath)
            Exit Sub
        End If

        MdlZTStudio.Trace("MdlBatch", "WriteAniFile", "Processing main folder " & StrPath)

        Dim StackDirectories As New Stack(Of String)

        StackDirectories.Push(StrPath)

        ' Continue processing for each stacked directory
        Do While (StackDirectories.Count > 0)

            ' Get top directory string
            Dim StrDirectoryName As String = StackDirectories.Pop

            Dim ObjAniFile As New ClsAniFile(StrDirectoryName & "\" & Path.GetFileName(StrDirectoryName) & ".ani")
            MdlZTStudio.Trace("MdlBatch", "WriteAniFile", "Attempting to create " & Path.GetFileName(StrDirectoryName) & ".ani")
            ObjAniFile.CreateAniConfig()

            ' Loop through all subdirectories and add them to the stack.
            Dim StrSubDirectoryName As String
            For Each StrSubDirectoryName In Directory.GetDirectories(StrDirectoryName)
                StackDirectories.Push(StrSubDirectoryName)
            Next

            ' Make sure everything is finished. Needed?
            Application.DoEvents()

        Loop

        ' Make sure everything is finished. Needed?
        Application.DoEvents()

        Exit Sub

dBug:
        MdlZTStudio.UnhandledError("MdlBatch", "WriteAniFile", Information.Err, True)

    End Sub

End Module
