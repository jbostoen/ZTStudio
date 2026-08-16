
Imports System.IO

''' <summary>
''' Groups batch operations
''' </summary>
Module MdlBatch

    ''' <summary>
    ''' Recursively enumerates every directory under StrRoot (StrRoot itself included first), lazily,
    ''' depth-first. Extracted so the same Stack(Of String)-based walk isn't duplicated across
    ''' MdlBatch.WriteAniFile, MdlTests.GetHashesOfFilesInFolder, and MdlZTStudioUI.UpdateExplorerPane
    ''' (see issue #63 - the same kind of extraction already done for the per-file batch loop in
    ''' MdlTasks.RunFileBatch, issue #52).
    ''' </summary>
    ''' <param name="StrRoot">Root directory to start from.</param>
    ''' <param name="BlnPreserveOrder">
    ''' If True, each directory's subdirectories are visited in the order Directory.GetDirectories()
    ''' returns them (typically alphabetical) - needed by callers that display results to the user
    ''' (e.g. building a TreeView). If False (the default), visitation order isn't guaranteed to
    ''' match that; every directory is still visited exactly once, which is all that matters for
    ''' callers that don't care about order (e.g. hashing every file, or writing a .ani per folder).
    ''' </param>
    Public Iterator Function EnumerateDirectoriesRecursive(StrRoot As String, Optional BlnPreserveOrder As Boolean = False) As IEnumerable(Of String)

        Dim StackDirectories As New Stack(Of String)
        StackDirectories.Push(StrRoot)

        Do While StackDirectories.Count > 0

            Dim StrCurrentDirectory As String = StackDirectories.Pop()
            Yield StrCurrentDirectory

            Dim ArrSubDirectories As String() = Directory.GetDirectories(StrCurrentDirectory)
            If BlnPreserveOrder = True Then
                Array.Reverse(ArrSubDirectories)
            End If

            For Each StrSubDirectory As String In ArrSubDirectories
                StackDirectories.Push(StrSubDirectory)
            Next

        Loop

    End Function

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

            ' This is only ever called from the batch conversion/offset-fix loops in MdlTasks (which run
            ' on a background thread via Task.Run), so one folder with an unexpected layout (e.g. a
            ' malformed/corrupt graphic) should not abort .ani generation for the rest of the tree, nor
            ' should it pop up a blocking dialog on a background thread. Same reasoning as issue #44,
            ' applied here via the same BlnBatchOperationRunning flag.
            BlnBatchOperationRunning = True
            Try

                For Each StrDirectoryName As String In EnumerateDirectoriesRecursive(StrPath)

                    Try
                        Dim ObjAniFile As New ClsAniFile(StrDirectoryName & "\" & Path.GetFileName(StrDirectoryName) & ".ani")
                        MdlZTStudio.Trace("MdlBatch", "WriteAniFile", "Attempting to create " & Path.GetFileName(StrDirectoryName) & ".ani")
                        ObjAniFile.CreateAniConfig()

                    Catch exDirectory As Exception
                        MdlZTStudio.LogError("MdlBatch", "WriteAniFile", "Failed to create .ani file for folder: " & StrDirectoryName, exDirectory)
                    End Try

                Next

            Finally
                BlnBatchOperationRunning = False
            End Try

        Catch ex As Exception
            MdlZTStudio.UnhandledError("MdlBatch", "WriteAniFile", ex, True)
        End Try

    End Sub

End Module
