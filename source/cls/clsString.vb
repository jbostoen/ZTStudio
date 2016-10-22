Imports System.Runtime.CompilerServices

Module StringExtensions

    <Extension()>
    Public Function ReverseHEX(ByVal aString As String) As String

        Dim retString As String
         
        Dim lst As New List(Of String)
        lst.AddRange(Enumerable.Range(0, aString.Length / 2).Select(Function(x) aString.Substring(x * 2, 2)).ToList())
        lst.Reverse()
         

        retString = Strings.Join(lst.ToArray(), " ")


        ' return reverse, with spaces
        Return Strings.Trim(retString)


    End Function



End Module