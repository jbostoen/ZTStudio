
Public Class List(Of T)
    Inherits Generic.List(Of T)


    Public Overloads Sub Add(ByVal item As T, Optional forceUpdate As Boolean = True)

        MyBase.Add(item)

        If forceUpdate = True Then
            'clsTasks.update_Info("List - item added. Overload.")
        End If




    End Sub

    Public Overloads Sub AddRange(ByVal range As System.Collections.Generic.IEnumerable(Of T), Optional forceUpdate As Boolean = True)

        MyBase.AddRange(range)

        If forceUpdate = True Then
            'clsTasks.update_Info("List - item range added. Overload.")
        End If

    End Sub

    Public Overloads Sub Insert(index As Integer, item As T, Optional forceUpdate As Boolean = True)

        MyBase.Insert(index, item)

        If forceUpdate = True Then
            clsTasks.update_Info("List - item inserted. Overload.")
        End If

    End Sub

    Public Overloads Sub Remove(item As T, Optional forceUpdate As Boolean = True)

        MyBase.Remove(item)

        If forceUpdate = True Then
            clsTasks.update_Info("List - item added. Removed.")
        End If

    End Sub
    Public Overloads Sub RemoveAt(index As Integer, Optional forceUpdate As Boolean = True)

        MyBase.RemoveAt(index)

        If forceUpdate = True Then
            clsTasks.update_Info("List - item added. Removed at.")
        End If
    End Sub

    Public Overloads Sub Clear(Optional forceUpdate As Boolean = True)

        MyBase.Clear()

        If forceUpdate = True Then
            clsTasks.update_Info("List - item added. Cleared.")
        End If

    End Sub

End Class


