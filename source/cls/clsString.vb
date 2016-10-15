Imports System.Runtime.CompilerServices

Module StringExtensions

    <Extension()>
    Public Function ReverseHEX(ByVal aString As String) As String

        Dim retString As String

        Dim lst As List(Of String) = Strings.Split(aString, " ").ToList()
        lst.Reverse()


        retString = Strings.Join(lst.ToArray(), " ")


        ' return reverse, with spaces
        Return Strings.Trim(retString)


    End Function



End Module