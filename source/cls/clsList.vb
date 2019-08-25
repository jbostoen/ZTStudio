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

    Public Overloads Sub Remove(item As T, Optional BlnForceUpdateInfo As Boolean = True)

        MyBase.Remove(item)

        If BlnForceUpdateInfo = True Then
            MdlZTStudioUI.UpdateFrameInfo("List - item added. Removed.")
        End If

    End Sub
    Public Overloads Sub RemoveAt(index As Integer, Optional BlnForceUpdateInfo As Boolean = True)

        MyBase.RemoveAt(index)

        If BlnForceUpdateInfo = True Then
            MdlZTStudioUI.UpdateFrameInfo("List - item added. Removed at.")
        End If
    End Sub

    Public Overloads Sub Clear(Optional BlnForceUpdateInfo As Boolean = True)

        MyBase.Clear()

        If BlnForceUpdateInfo = True Then
            MdlZTStudioUI.UpdateFrameInfo("List - item added. Cleared.")
        End If

    End Sub

    ''' <summary>
    ''' Number of items to remove (from the start)
    ''' </summary>
    ''' <param name="IntItems">Number of items to remove (from the start)</param>
    Public Overloads Sub Skip(ByVal IntItems As Integer)

        MyBase.RemoveRange(0, IntItems)

    End Sub

End Class


