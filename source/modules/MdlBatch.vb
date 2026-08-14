
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

        Try

            If Cfg_export_ZT1_Ani = 0 Then

                MdlZTStudio.Trace("MdlBatch", "WriteAniFile", "Option to create .ani not enabled. Skipping main folder " & StrPath)
                Exit Sub
            End If

            MdlZTStudio.Trace("MdlBatch", "WriteAniFile", "Processing main folder " & StrPath)

            Dim StackDirectories As New Stack(Of String)

            StackDirectories.Push(StrPath)

            ' This is only ever called from the batch conversion/offset-fix loops in MdlTasks (which run
            ' on a background thread via Task.Run), so one folder with an unexpected layout (e.g. a
            ' malformed/corrupt graphic) should not abort .ani generation for the rest of the tree, nor
            ' should it pop up a blocking dialog on a background thread. Same reasoning as issue #44,
            ' applied here via the same BlnBatchOperationRunning flag.
            BlnBatchOperationRunning = True
            Try

                ' Continue processing for each stacked directory
                Do While (StackDirectories.Count > 0)

                    ' Get top directory string
                    Dim StrDirectoryName As String = StackDirectories.Pop

                    Try
                        Dim ObjAniFile As New ClsAniFile(StrDirectoryName & "\" & Path.GetFileName(StrDirectoryName) & ".ani")
                        MdlZTStudio.Trace("MdlBatch", "WriteAniFile", "Attempting to create " & Path.GetFileName(StrDirectoryName) & ".ani")
                        ObjAniFile.CreateAniConfig()

                    Catch exDirectory As Exception
                        MdlZTStudio.LogError("MdlBatch", "WriteAniFile", "Failed to create .ani file for folder: " & StrDirectoryName, exDirectory)
                    End Try

                    ' Loop through all subdirectories and add them to the stack.
                    Dim StrSubDirectoryName As String
                    For Each StrSubDirectoryName In Directory.GetDirectories(StrDirectoryName)
                        StackDirectories.Push(StrSubDirectoryName)
                    Next

                Loop

            Finally
                BlnBatchOperationRunning = False
            End Try

        Catch ex As Exception
            MdlZTStudio.UnhandledError("MdlBatch", "WriteAniFile", ex, True)
        End Try

    End Sub

End Module
