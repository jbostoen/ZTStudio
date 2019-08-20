
Public Class List(Of T)
    Inherits Generic.List(Of T)


    Public Overloads Sub Add(ByVal item As T, Optional ForceUpdate As Boolean = True)

        MyBase.Add(item)

        If ForceUpdate = True Then
            'clsTasks.update_Info("List - item added. Overload.")
        End If


    End Sub

    Public Overloads Sub AddRange(ByVal range As System.Collections.Generic.IEnumerable(Of T), Optional ForceUpdate As Boolean = True)

        MyBase.AddRange(range)

        If ForceUpdate = True Then
            'clsTasks.update_Info("List - item range added. Overload.")
        End If

    End Sub

    Public Overloads Sub Insert(index As Integer, item As T, Optional ForceUpdate As Boolean = True)

        MyBase.Insert(index, item)

        If ForceUpdate = True Then
            MdlZTStudioUI.Updateinfo("List - item inserted. Overload.")
        End If

    End Sub

    Public Overloads Sub Remove(item As T, Optional ForceUpdate As Boolean = True)

        MyBase.Remove(item)

        If ForceUpdate = True Then
            MdlZTStudioUI.Updateinfo("List - item added. Removed.")
        End If

    End Sub
    Public Overloads Sub RemoveAt(index As Integer, Optional ForceUpdate As Boolean = True)

        MyBase.RemoveAt(index)

        If ForceUpdate = True Then
            MdlZTStudioUI.Updateinfo("List - item added. Removed at.")
        End If
    End Sub

    Public Overloads Sub Clear(Optional ForceUpdate As Boolean = True)

        MyBase.Clear()

        If ForceUpdate = True Then
            MdlZTStudioUI.Updateinfo("List - item added. Cleared.")
        End If

    End Sub

    Public Overloads Sub Skip(ByVal intItems As Integer)

        MyBase.RemoveRange(0, intItems)

    End Sub

End Class


