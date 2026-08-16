
Imports System.IO


''' <summary>
''' Some methods related to color palettes
''' </summary>
Module MdlColorPalette

    ''' <summary>
    ''' Replaces color (specified by index) in the main color palette
    ''' </summary>
    ''' <param name="IntIndex">Index of color to be replaced</param>
    Sub ReplaceColor(IntIndex As Integer)

        With FrmMain.DlgColor
            .Color = FrmMain.DgvPaletteMain.Rows(IntIndex).DefaultCellStyle.BackColor

            .AllowFullOpen = True
            .FullOpen = True
            .SolidColorOnly = True
            .ShowDialog()

        End With

        EditorGraphic.ColorPalette.ReplaceColorAt(IntIndex, FrmMain.DlgColor.Color)

        'Update entire palette (easy)
        EditorGraphic.ColorPalette.FillPaletteGrid(FrmMain.DgvPaletteMain)

    End Sub

    ''' <summary>
    ''' Moves color in the palette to a new position.
    ''' This has repercussions: order of colors changes, hex values (color index) need to be updated!
    ''' </summary>
    ''' <param name="IntIndexNow">Current index</param>
    ''' <param name="IntIndexDest">Wanted index</param>
    Sub MoveColor(IntIndexNow As Integer, IntIndexDest As Integer)

        ' MoveColorTo() handles the cache invalidation and the per-frame CoreImageHex regeneration
        ' (color indexes have changed) internally - see issue #61.
        EditorGraphic.ColorPalette.MoveColorTo(IntIndexNow, IntIndexDest)

        ' Refresh
        EditorGraphic.ColorPalette.FillPaletteGrid(FrmMain.DgvPaletteMain)

    End Sub

    ''' <summary>
    ''' Adds a new color entry at the specified index. 
    ''' The color hasn't been picked yet, so by default it's transparent.
    ''' </summary>
    ''' <param name="IntIndexNow">Index</param>
    Sub AddColor(IntIndexNow As Integer)

        If EditorGraphic.ColorPalette.Colors.Count = 256 Then
            MdlZTStudio.HandledError("MdlColorPalette", "AddColor", "You can't add any more colors to this palette." & vbCrLf & "The maximum of 255 (+1 transparent) colors has been reached.")
        End If

        ' Get color
        Dim ObjColor As System.Drawing.Color = Cfg_Grid_BackGroundColor

        With FrmMain.DlgColor
            .Color = ObjColor

            .AllowFullOpen = True
            .FullOpen = True
            .SolidColorOnly = True
            .ShowDialog()

        End With

        ObjColor = FrmMain.DlgColor.Color

        ' Insert it at the position we want.
        EditorGraphic.ColorPalette.AddColorAt(IntIndexNow + 1, ObjColor)

        ' Refresh
        EditorGraphic.ColorPalette.FillPaletteGrid(FrmMain.DgvPaletteMain)


    End Sub


    ''' <summary>
    ''' Opens a color palette, displays it as a separate window.
    ''' </summary>
    ''' <param name="StrFileName">Source filename of the ZT1 color palette (.pal)</param>
    Sub LoadPalette(StrFileName As String)

        Try

            If File.Exists(StrFileName) Then

                If Path.GetExtension(StrFileName) <> ".pal" Then
                    MdlZTStudio.HandledError("MdlColorPalette", "LoadPalette", "You did not select a ZT1 Color Palette (.pal file).")

                Else

                    ' Show a dedicated window
                    Dim CpPallete As New ClsPalette(Nothing)
                    Dim FrmColPal As New FrmPal

                    ' Read the .pal file. ReadPal() has no Try/Catch of its own (unlike ClsGraphic.Read()),
                    ' so a malformed/truncated file that trips something other than its explicit length
                    ' check would otherwise propagate straight out of this button click.
                    CpPallete.ReadPal(StrFileName)
                    CpPallete.FillPaletteGrid(FrmColPal.DgvPal)

                    FrmColPal.SsFileName.Text = Path.GetFileName(StrFileName)

                    FrmColPal.Show()

                End If

            Else
                MdlZTStudio.HandledError("MdlColorPalette", "LoadPalette", "Could not find '" & StrFileName & "'")
            End If

        Catch ex As Exception
            MdlZTStudio.HandledError("MdlColorPalette", "LoadPalette", "Could not open color palette '" & StrFileName & "': " & ex.Message, False, ex, ZTStudioErrorCategory.FileFormat)
        End Try

    End Sub
End Module
