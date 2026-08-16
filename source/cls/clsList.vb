''' <summary>
''' List class. Originally contained an additional BlnForceUpdateInfo Parameter on most main methods, but the function has been deprecated.
''' Since it may be useful however, it has not been removed as of now.
''' </summary>
''' <typeparam name="T"></typeparam>
Public Class List(Of T)
    Inherits Generic.List(Of T)

    ''' <summary>
    ''' Implements a custom parameter. Deprecated?
    ''' </summary>
    ''' <param name="Item"></param>
    ''' <param name="BlnForceUpdateInfo"></param>
    Public Overloads Sub Add(ByVal Item As T, Optional BlnForceUpdateInfo As Boolean = True)

        MyBase.Add(Item)

        If BlnForceUpdateInfo = True Then
            'MdlZTStudioUI.UpdateInfo("List - item added. Overload.")
        End If


    End Sub

    ''' <summary>
    ''' Implements a custom parameter. Deprecated?
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="BlnForceUpdateInfo"></param>
    Public Overloads Sub AddRange(ByVal Range As System.Collections.Generic.IEnumerable(Of T), Optional BlnForceUpdateInfo As Boolean = True)

        MyBase.AddRange(Range)

        If BlnForceUpdateInfo = True Then
            'MdlZTStudioUI.UpdateInfo("List - item range added. Overload.")
        End If

    End Sub

    ''' <summary>
    ''' Implements a custom parameter. Deprecated?
    ''' </summary>
    ''' <param name="Index"></param>
    ''' <param name="Item"></param>
    ''' <param name="BlnForceUpdateInfo"></param>
    Public Overloads Sub Insert(Index As Integer, Item As T, Optional BlnForceUpdateInfo As Boolean = True)

        MyBase.Insert(Index, Item)

        If BlnForceUpdateInfo = True Then
            'MdlZTStudioUI.UpdateInfo("List - item inserted. Overload.")
        End If

    End Sub

    ''' <summary>
    ''' Implements a custom parameter, now defaulting to False (previously True).
    ''' </summary>
    ''' <remarks>
    ''' This generic list type is reused for unrelated data across the codebase (directory/view
    ''' lists, a color palette, drawing-instruction pixel colors, ...), not just the main editor's
    ''' frame collection - defaulting to True meant any call to Remove/RemoveAt/Clear anywhere that
    ''' didn't explicitly pass False silently reached into FrmMain's UI controls. Since batch
    ''' operations run on a background thread (Task.Run, see issue #48), and MdlZTStudioUI.UpdateFrameInfo
    ''' has no cross-thread marshaling of its own, this was a real, reachable bug: ClsGraphic.Read()
    ''' calls Me.ColorPalette.Colors.Clear() (bare, relying on this default) whenever a graphic's
    ''' color palette differs from the previously loaded one - during a batch ZT1-to-PNG conversion of
    ''' a folder containing graphics with different palettes, this fired UpdateFrameInfo() from the
    ''' background batch thread, which would throw InvalidOperationException ("cross-thread operation
    ''' not valid") - silently caught and logged as a per-file failure by the batch loop rather than
    ''' visibly crashing, which is likely why this had gone unnoticed. Defaulting to False (and
    ''' requiring call sites that genuinely want the callback to opt in explicitly) closes this off
    ''' for both the known call site and any future one. See issue #62.
    ''' </remarks>
    Public Overloads Sub Remove(item As T, Optional BlnForceUpdateInfo As Boolean = False)

        MyBase.Remove(item)

        If BlnForceUpdateInfo = True Then
            MdlZTStudioUI.UpdateFrameInfo("List - item added. Removed.")
        End If

    End Sub
    Public Overloads Sub RemoveAt(index As Integer, Optional BlnForceUpdateInfo As Boolean = False)

        MyBase.RemoveAt(index)

        If BlnForceUpdateInfo = True Then
            MdlZTStudioUI.UpdateFrameInfo("List - item added. Removed at.")
        End If
    End Sub

    Public Overloads Sub Clear(Optional BlnForceUpdateInfo As Boolean = False)

        MyBase.Clear()

        If BlnForceUpdateInfo = True Then
            MdlZTStudioUI.UpdateFrameInfo("List - item added. Cleared.")
        End If

    End Sub

    ''' <summary>
    ''' Number of items to remove (from the start)
    ''' </summary>
    ''' <param name="IntItems">Number of items to remove (from the start)</param>
    ''' <remarks>Mutates the list in place by removing items; unlike LINQ's Skip(), it does not return a filtered sequence.</remarks>
    Public Sub RemoveFirst(ByVal IntItems As Integer)

        MyBase.RemoveRange(0, IntItems)

    End Sub

End Class


