Public Class frmGIMPRecolor

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click


        Dim pal As clsPalette = editorGraphic.colorPalette


        If System.IO.File.Exists(Application.ExecutablePath & "\tmp_pal.png") = True Then
            System.IO.File.Delete(Application.ExecutablePath & "\tmp_pal.png")
        End If
        If System.IO.File.Exists(Application.ExecutablePath & "\tmp_pal_orig.png") = True Then
            System.IO.File.Delete(Application.ExecutablePath & "\tmp_pal_orig.png")
        End If

        ' Save palette to .PNG
        pal.export_to_PNG(System.IO.Path.GetDirectoryName(Application.ExecutablePath) & "\tmp_pal_orig.png")
        pal.export_to_PNG(System.IO.Path.GetDirectoryName(Application.ExecutablePath) & "\tmp_pal.png")


        ' Do the gimp stuff.
        'Shell("c:\Program Files\GIMP 2\bin\gimp-2.8.exe -b ""(script-ztstudio \""c:\\users\\jeffrey\\desktop\\test.png\"" 0 0 0 0 0 0)""")

        Dim sFile As String = System.IO.Path.GetDirectoryName(Application.ExecutablePath) & "\tmp_pal.png"
        sFile = sFile.Replace("\", "\\")


        Dim intTones As Byte
        Select Case cboTones.SelectedValue
            Case "Shadows" : intTones = 0
            Case "Midtones" : intTones = 1
            Case "Highlights" : intTones = 2

        End Select

        Dim objProcess As System.Diagnostics.Process
        Try
            objProcess = New System.Diagnostics.Process()
            objProcess.StartInfo.FileName = "c:\Program Files\GIMP 2\bin\gimp-console-2.8.exe"
            objProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            objProcess.StartInfo.Arguments = "-i -b ""(script-ztstudio \""" & sFile & "\"" 0 " & _
                tbRed.Value & " " & _
                tbGreen.Value & " " & _
                tbBlue.Value & " " & _
                tbBrightness.Value & " " & _
                tbContrast.Value & ")"""""
            objProcess.Start()

            'Wait until the process passes back an exit code 
            objProcess.WaitForExit()

            'Free resources associated with this process
            objProcess.Close()
        Catch
            MessageBox.Show("Could not start process ", "Error")
        End Try


        Application.DoEvents()


        Debug.Print("Processed?")

        ' Read palette from .PNG  
        ' To avoid triggering an immediate redraw/remove cache, we need to make sure all colors are there right away.
        Dim pal2 As New clsPalette
        pal2.import_from_PNG(System.IO.Path.GetDirectoryName(Application.ExecutablePath) & "\tmp_pal.png", True)
        pal.colors = pal2.colors

        editorGraphic.lastUpdated = Now.ToString("yyyyMMddHHmmss")


        ' Update the palette preview.
        pal2.fillPaletteGrid(frmMain.dgvPaletteMain)



        ' And finally, update preview
        clsTasks.preview_update()


        Me.Close()

        'Me.Hide()

       
    End Sub

    Private Sub frmGIMPRecolor_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        cboTones.SelectedIndex = 0



    End Sub
End Class