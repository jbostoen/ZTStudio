Imports System.Runtime.CompilerServices

Module StringExtensions

    <Extension()>
    Public Function ReverseHEX(ByVal aString As String) As String

        ' Input spaces or not? It doesn't matter.
        aString = Strings.Replace(aString, " ", "")

        Dim retString As String = ""

        While aString.Length >= 2
            retString = retString & Strings.Right(aString, 2) & " "
            aString = Strings.Left(aString, aString.Length - 2)
        End While

        ' return reverse, with spaces
        Return Strings.Trim(retString)


    End Function



End Module