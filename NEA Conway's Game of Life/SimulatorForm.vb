Public Class SimulatorForm

    'Create an object myGrid from class GOLGrid
    Private myGrid As GOLGrid

    Private gridSize As Integer = 230
    Private gridLength As Integer

    Const maxLength As Integer = 700
    Private minCellSize As Integer
    Private maxCellSize As Integer

    Private myPanel As Panel
    Private myTimer As Timer

    Private isRun As Boolean = False
    Private isDrag As Boolean = False

    Private generation As Integer = 0
    Private infoLabel As Label

    Private ButtonRun As Button
    Private ButtonStep As New Button
    Private ButtonClear As New Button
    Private ButtonRewind As New Button
    Private ButtonUndo As New Button
    Private ButtonSkip As New Button

    Private selectLabel As Label
    Private ButtonSelect As New CheckBox
    Private ButtonCopy As New Button
    Private ButtonPaste As New Button

    Private speedTrackbar As TrackBar
    Private speedLabel As Label
    Private scaleTrackbar As TrackBar
    Private scaleLabel As Label
    Private borderWidthTrackbar As TrackBar
    Private borderWidthLabel As Label

    Private colourLabel As Label
    Private ButtonCellColour As New Button
    Private ButtonDeadCellColour As New Button
    Private ButtonBorderColour As New Button

    Private rulesPanel As Panel
    Private liveTextBox As TextBox
    Private bornTextBox As TextBox
    Dim rulesLabel As Label
    Dim liveLabel1 As Label
    Dim liveLabel2 As Label
    Dim bornLabel1 As Label
    Dim bornLabel2 As Label

    Const conwayLiveNum As String = "23"
    Const conwayBornNum As String = "3"
    Const replicatorLiveNum As String = "1357"
    Const replicatorBornNum As String = "1357"
    Const mazeLiveNum As String = "12345"
    Const mazeBornNum As String = "3"
    Const twoLiveNum As String = "125"
    Const twoBornNum As String = "36"
    Const HLLiveNum As String = "23"
    Const HLBornNum As String = "36"
    Private conwayLabel As Label
    Private replicatorLabel As Label
    Private mazeLabel As Label
    Private twoLabel As Label
    Private HLLabel As Label

    Private undoStack As New Stack

    Private selected As Boolean = False
    Public selectedCells(,) As Boolean

    Private myForm4 As EditingForm

    Public Sub New(ByVal Size As Integer)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        gridSize = Size
        'gridLength = 700

        minCellSize = maxLength \ Size
        gridLength = minCellSize * Size
        maxCellSize = minCellSize * 10

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Height = 800
        Width = 1200
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.ControlBox = False
        Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        Me.BackColor = Color.Black
        Me.ForeColor = Color.White
        Me.DoubleBuffered = True
        Me.Text = "Conway's Game of Life"

        myPanel = New Panel()
        myPanel.Height = gridLength
        myPanel.Width = gridLength
        myPanel.Top = (maxLength - myPanel.Height) / 2 + 20
        myPanel.Left = (maxLength - myPanel.Width) / 2 + 20
        myPanel.BorderStyle = BorderStyle.None
        Me.Controls.Add(myPanel)
        myGrid = New GOLGrid()
        myGrid.createBoard(gridSize, gridSize, gridLength, gridLength, minCellSize)
        myPanel.Controls.Add(myGrid)

        Call createTimer()
        Call createBasicControls()
        Call CreateRules()
        Call createTrackbars()
        Call createColourSettings()
        Call createSelect()

        'Display running information
        infoLabel = New Label
        updateInfo()
        infoLabel.Height = 100
        infoLabel.Width = 700
        infoLabel.Font = New Font(infoLabel.Font.FontFamily, 10)
        infoLabel.Location = New Point(20, 730)
        Me.Controls.Add(infoLabel)

        Dim ButtonExport As New Button
        ButtonExport.Text = "Export"
        ButtonExport.Height = 30
        ButtonExport.Width = 70
        ButtonExport.Location = New Point(830, 80)
        AddHandler ButtonExport.Click, AddressOf ButtonExport_Click
        Me.Controls.Add(ButtonExport)

        Dim ButtonImport As New Button
        ButtonImport.Text = "Import"
        ButtonImport.Height = 30
        ButtonImport.Width = 70
        ButtonImport.Location = New Point(750, 80)
        AddHandler ButtonImport.Click, AddressOf ButtonImport_Click
        Me.Controls.Add(ButtonImport)

        'Create Random Button
        Dim ButtonRandom As New Button
        ButtonRandom.Text = "Randomise"
        ButtonRandom.Height = 30
        ButtonRandom.Width = 100
        ButtonRandom.Location = New Point(935, 80)
        AddHandler ButtonRandom.Click, AddressOf ButtonRandom_Click
        Me.Controls.Add(ButtonRandom)

        Dim edgeLabel As Label
        edgeLabel = New Label
        edgeLabel.Text = "Boundary condition:"
        edgeLabel.Width = 200
        edgeLabel.Location = New Point(750, 520)
        Me.Controls.Add(edgeLabel)

        Dim comboBoxEdge As ComboBox
        comboBoxEdge = New ComboBox
        comboBoxEdge.Location = New Point(750, 550)
        comboBoxEdge.Width = 200
        comboBoxEdge.DropDownStyle = ComboBoxStyle.DropDownList
        AddHandler comboBoxEdge.SelectedIndexChanged, AddressOf comboBoxEdge_SelectedIndexChanged
        Me.Controls.Add(comboBoxEdge)
        comboBoxEdge.Items.Add("Closed Boundaries")
        comboBoxEdge.Items.Add("Toroidal Boundaries")
        comboBoxEdge.SelectedIndex = 0
        myGrid.boundaryCondition = "Closed Boundaries"

        'Return to home page
        Dim ButtonBack As New Button
        ButtonBack.Text = "Return to home page"
        ButtonBack.BackColor = Color.Black
        ButtonBack.ForeColor = Color.White
        ButtonBack.Width = 190
        ButtonBack.Height = 30
        ButtonBack.TextAlign = ContentAlignment.MiddleCenter
        ButtonBack.Font = New Font("Cambria", 10)
        ButtonBack.Location = New Point(960, 710)
        AddHandler ButtonBack.Click, AddressOf ButtonBack_Click
        Me.Controls.Add(ButtonBack)

        Dim ButtonAbout As New Button
        ButtonAbout.Text = "About the simulator"
        ButtonAbout.BackColor = Color.Black
        ButtonAbout.ForeColor = Color.White
        ButtonAbout.Width = 185
        ButtonAbout.Height = 30
        ButtonAbout.TextAlign = ContentAlignment.MiddleCenter
        ButtonAbout.Font = New Font("Cambria", 10)
        ButtonAbout.Location = New Point(750, 710)
        AddHandler ButtonAbout.Click, AddressOf ButtonAbout_Click
        Me.Controls.Add(ButtonAbout)

        For Each control In Me.Controls
            Dim myFont As New Font("Courier New", 10.5)
            control.Font = myFont
        Next

        conwayLabel.Font = New Font("Courier New", 10.5, FontStyle.Italic Or FontStyle.Underline)
        replicatorLabel.Font = New Font("Courier New", 10.5, FontStyle.Italic Or FontStyle.Underline)
        mazeLabel.Font = New Font("Courier New", 10.5, FontStyle.Italic Or FontStyle.Underline)
        twoLabel.Font = New Font("Courier New", 10.5, FontStyle.Italic Or FontStyle.Underline)
        HLLabel.Font = New Font("Courier New", 10.5, FontStyle.Italic Or FontStyle.Underline)
    End Sub

    Private Sub createTimer()
        myTimer = New Timer
        myTimer.Interval = 100

        AddHandler myTimer.Tick, AddressOf myTimer_Tick
    End Sub

    Private Sub createBasicControls()
        ' Create Run Button
        ButtonRun = New Button
        ButtonRun.Text = "RUN"
        ButtonRun.Height = 30
        ButtonRun.Width = 55
        ButtonRun.Location = New Point(750, 30)
        ButtonRun.FlatStyle = False
        ButtonRun.BackColor = Color.Goldenrod
        ButtonRun.ForeColor = Color.White
        ButtonRun.FlatAppearance.BorderSize = 0
        AddHandler ButtonRun.Click, AddressOf ButtonRun_Click
        Me.Controls.Add(ButtonRun)

        ' Create Step Button
        ButtonStep.Text = "STEP"
        ButtonStep.Height = 30
        ButtonStep.Width = 55
        ButtonStep.Location = New Point(815, 30)
        ButtonStep.FlatStyle = False
        ButtonStep.BackColor = Color.Gray
        ButtonStep.FlatAppearance.BorderSize = 0
        AddHandler ButtonStep.Click, AddressOf ButtonStep_Click
        Me.Controls.Add(ButtonStep)

        'Create Clear Button
        ButtonClear.Text = "CLEAR"
        ButtonClear.Height = 30
        ButtonClear.Width = 65
        ButtonClear.Location = New Point(880, 30)
        ButtonClear.FlatStyle = False
        ButtonClear.BackColor = Color.Gray
        ButtonClear.FlatAppearance.BorderSize = 0
        AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
        Me.Controls.Add(ButtonClear)

        ' Create Rewind Button
        ButtonRewind.Text = "REWIND"
        ButtonRewind.Height = 30
        ButtonRewind.Width = 80
        ButtonRewind.Location = New Point(955, 30)
        ButtonRewind.FlatStyle = False
        ButtonRewind.BackColor = Color.Gray
        ButtonRewind.FlatAppearance.BorderSize = 0
        AddHandler ButtonRewind.Click, AddressOf ButtonRewind_Click
        Me.Controls.Add(ButtonRewind)

        ' Create Undo Button
        ButtonUndo.Text = "UNDO"
        ButtonUndo.Height = 30
        ButtonUndo.Width = 55
        ButtonUndo.Location = New Point(1045, 30)
        ButtonUndo.FlatStyle = False
        ButtonUndo.BackColor = Color.Gray
        ButtonUndo.FlatAppearance.BorderSize = 0
        AddHandler ButtonUndo.Click, AddressOf ButtonUndo_Click
        Me.Controls.Add(ButtonUndo)

        ' Create Skip Button
        ButtonSkip.Text = "SKIP"
        ButtonSkip.Height = 30
        ButtonSkip.Width = 55
        ButtonSkip.Location = New Point(1110, 30)
        ButtonSkip.FlatStyle = False
        ButtonSkip.BackColor = Color.Gray
        ButtonSkip.FlatAppearance.BorderSize = 0
        AddHandler ButtonSkip.Click, AddressOf ButtonSkip_Click
        Me.Controls.Add(ButtonSkip)
    End Sub

    Private Sub createTrackbars()
        'Create Speed Trackbar
        speedTrackbar = New TrackBar
        speedTrackbar.Location = New Point(750, 160)
        speedTrackbar.Text = "Speed"
        speedTrackbar.Width = 200
        speedTrackbar.SmallChange = 20
        speedTrackbar.LargeChange = 20
        speedTrackbar.TickFrequency = 20
        speedTrackbar.TickStyle = TickStyle.None
        speedTrackbar.Minimum = 1
        speedTrackbar.Maximum = 500
        speedTrackbar.Value = 100
        AddHandler speedTrackbar.Scroll, AddressOf speedTrackbar_Scroll
        Me.Controls.Add(speedTrackbar)
        'Speed label
        speedLabel = New Label
        speedLabel.Width = 200
        speedLabel.Text = String.Format("Time interval: {0} ms", speedTrackbar.Value)
        speedLabel.Location = New Point(750, 140)
        Me.Controls.Add(speedLabel)

        'Create Scale Trackbar
        scaleTrackbar = New TrackBar
        scaleTrackbar.Location = New Point(750, 220)
        scaleTrackbar.Text = "Scale"
        scaleTrackbar.Width = 200
        'scaleTrackbar.LargeChange = 0.1
        scaleTrackbar.TickStyle = TickStyle.None
        scaleTrackbar.SmallChange = 1
        scaleTrackbar.LargeChange = 2
        scaleTrackbar.Minimum = minCellSize
        scaleTrackbar.Maximum = maxCellSize
        scaleTrackbar.Value = minCellSize
        AddHandler scaleTrackbar.Scroll, AddressOf scaleTrackbar_Scroll
        Me.Controls.Add(scaleTrackbar)
        'Scale label
        scaleLabel = New Label
        scaleLabel.Width = 200
        scaleLabel.Text = String.Format("Scale:")
        scaleLabel.Location = New Point(750, 200)
        Me.Controls.Add(scaleLabel)

        'Create borderWidth Trackbar
        borderWidthTrackbar = New TrackBar
        borderWidthTrackbar.Location = New Point(750, 280)
        borderWidthTrackbar.Text = "Border Width"
        borderWidthTrackbar.Width = 200
        borderWidthTrackbar.TickStyle = TickStyle.None
        borderWidthTrackbar.SmallChange = 1
        borderWidthTrackbar.LargeChange = 1
        borderWidthTrackbar.Minimum = 0
        borderWidthTrackbar.Maximum = 5
        borderWidthTrackbar.Value = 1
        AddHandler borderWidthTrackbar.Scroll, AddressOf borderWidthTrackbar_Scroll
        Me.Controls.Add(borderWidthTrackbar)
        'borderWidth label
        borderWidthLabel = New Label
        borderWidthLabel.Width = 200
        borderWidthLabel.Text = String.Format("Border Width:")
        borderWidthLabel.Location = New Point(750, 260)
        Me.Controls.Add(borderWidthLabel)
    End Sub

    Private Sub createColourSettings()
        colourLabel = New Label
        colourLabel.Width = 200
        colourLabel.Text = String.Format("Colour settings:")
        colourLabel.Location = New Point(990, 140)
        colourLabel.Font = New Font("Cambria", 10)
        Me.Controls.Add(colourLabel)

        ButtonCellColour.Text = "Live Cell"
        ButtonCellColour.Height = 30
        ButtonCellColour.Width = 100
        ButtonCellColour.Location = New Point(990, 165)
        ButtonCellColour.FlatStyle = False
        ButtonCellColour.BackColor = Color.Silver
        ButtonCellColour.ForeColor = Color.Black
        AddHandler ButtonCellColour.Click, AddressOf ButtonCellColour_Click
        Me.Controls.Add(ButtonCellColour)

        ButtonDeadCellColour.Text = "Dead Cell"
        ButtonDeadCellColour.Height = 30
        ButtonDeadCellColour.Width = 100
        ButtonDeadCellColour.Location = New Point(990, 205)
        ButtonDeadCellColour.FlatStyle = False
        ButtonDeadCellColour.BackColor = Color.Silver
        ButtonDeadCellColour.ForeColor = Color.Black
        AddHandler ButtonDeadCellColour.Click, AddressOf ButtonDeadCellColour_Click
        Me.Controls.Add(ButtonDeadCellColour)

        ButtonBorderColour.Text = "Border"
        ButtonBorderColour.Height = 30
        ButtonBorderColour.Width = 100
        ButtonBorderColour.Location = New Point(990, 245)
        ButtonBorderColour.FlatStyle = False
        ButtonBorderColour.BackColor = Color.Silver
        ButtonBorderColour.ForeColor = Color.Black
        AddHandler ButtonBorderColour.Click, AddressOf ButtonBorderColour_Click
        Me.Controls.Add(ButtonBorderColour)
    End Sub

    Private Sub CreateRules()
        rulesLabel = New Label
        rulesLabel.Text = "Rules settings:"
        rulesLabel.Width = 200
        rulesLabel.Location = New Point(750, 330)
        Me.Controls.Add(rulesLabel)

        rulesPanel = New Panel()
        rulesPanel.Height = 135
        rulesPanel.Width = 290
        rulesPanel.Top = 360
        rulesPanel.Left = 750
        rulesPanel.BackColor = Color.DarkRed
        rulesPanel.BorderStyle = BorderStyle.Fixed3D
        Me.Controls.Add(rulesPanel)

        liveLabel1 = New Label
        liveLabel1.Text = "1. A live cell survives if it has"
        liveLabel1.ForeColor = Color.White
        liveLabel1.Top = 5
        liveLabel1.Left = 5
        liveLabel1.Width = 320

        liveLabel2 = New Label
        liveLabel2.Text = "live neighbours."
        liveLabel2.ForeColor = Color.White
        liveLabel2.Top = 32
        liveLabel2.Left = 150
        liveLabel2.Width = 160

        liveTextBox = New TextBox
        liveTextBox.Top = 30
        liveTextBox.Left = 35
        liveTextBox.Text = "23"
        AddHandler liveTextBox.TextChanged, AddressOf liveTextBox_TextChanged

        bornLabel1 = New Label
        bornLabel1.Text = "2. A dead cell is born if it has"
        bornLabel1.ForeColor = Color.White
        bornLabel1.Top = 75
        bornLabel1.Left = 5
        bornLabel1.Width = 320

        bornLabel2 = New Label
        bornLabel2.Text = "live neighbours."
        bornLabel2.ForeColor = Color.White
        bornLabel2.Top = 102
        bornLabel2.Left = 150
        bornLabel2.Width = 160

        bornTextBox = New TextBox
        bornTextBox.Top = 100
        bornTextBox.Left = 35
        bornTextBox.Text = "3"
        AddHandler bornTextBox.TextChanged, AddressOf bornTextBox_TextChanged

        rulesPanel.Controls.Add(liveLabel1)
        rulesPanel.Controls.Add(liveTextBox)
        rulesPanel.Controls.Add(liveLabel2)
        rulesPanel.Controls.Add(bornLabel1)
        rulesPanel.Controls.Add(bornTextBox)
        rulesPanel.Controls.Add(bornLabel2)

        conwayLabel = New Label
        conwayLabel.Text = "Conway's Life"
        conwayLabel.Location = New Point(1050, 360)
        conwayLabel.Width = 200
        AddHandler conwayLabel.Click, AddressOf conwayLabel_Click

        replicatorLabel = New Label
        replicatorLabel.Text = "Replicator"
        replicatorLabel.Location = New Point(1050, 390)
        replicatorLabel.Width = 200
        AddHandler replicatorLabel.Click, AddressOf replicatorLabel_Click

        mazeLabel = New Label
        mazeLabel.Text = "Maze"
        mazeLabel.Location = New Point(1050, 420)
        mazeLabel.Width = 200
        AddHandler mazeLabel.Click, AddressOf mazeLabel_Click

        twoLabel = New Label
        twoLabel.Text = "2x2"
        twoLabel.Location = New Point(1050, 450)
        twoLabel.Width = 200
        AddHandler twoLabel.Click, AddressOf twoLabel_Click

        HLLabel = New Label
        HLLabel.Text = "HighLife"
        HLLabel.Location = New Point(1050, 480)
        HLLabel.Width = 200
        AddHandler HLLabel.Click, AddressOf HLLabel_Click

        Me.Controls.Add(conwayLabel)
        Me.Controls.Add(replicatorLabel)
        Me.Controls.Add(mazeLabel)
        Me.Controls.Add(twoLabel)
        Me.Controls.Add(HLLabel)
    End Sub

    Private Sub createSelect()
        selectLabel = New Label
        selectLabel.Text = "Copy and Paste:"
        selectLabel.Width = 200
        selectLabel.Location = New Point(750, 610)
        Me.Controls.Add(selectLabel)

        ButtonSelect.Text = "Select"
        ButtonSelect.Location = New Point(750, 640)
        AddHandler ButtonSelect.CheckedChanged, AddressOf ButtonSelect_Checked
        Me.Controls.Add(ButtonSelect)

        'Copy
        ButtonCopy.Text = "Copy"
        ButtonCopy.Height = 30
        ButtonCopy.Width = 70
        ButtonCopy.Location = New Point(870, 640)
        AddHandler ButtonCopy.Click, AddressOf ButtonCopy_Click
        Me.Controls.Add(ButtonCopy)

        'Paste
        ButtonPaste.Text = "Paste"
        ButtonPaste.Height = 30
        ButtonPaste.Width = 70
        ButtonPaste.Location = New Point(950, 640)
        AddHandler ButtonPaste.Click, AddressOf ButtonPaste_Click
        Me.Controls.Add(ButtonPaste)
    End Sub

    ' Update the simulation at discrete time steps
    Private Sub myTimer_Tick(ByVal Sender As Object, ByVal e As EventArgs)
        myGrid.updateGridToNextState()
        generation += 1
        updateInfo()
        myGrid.Invalidate()
    End Sub

    ' When this checkbox is checked, the user can select patterns on grid
    Protected Sub ButtonSelect_Checked(sender As Object, e As EventArgs)
        If sender.checked Then
            myGrid.isSelect = True
        Else
            myGrid.isSelect = False
            myGrid.Invalidate()
        End If
    End Sub


    Private Sub ButtonRun_Click(sender As Object, e As EventArgs)
        isRun = Not isRun
        If isRun Then
            ' Objective 4. a.
            ButtonRun.Text = "STOP"
            myTimer.Enabled = True
            myTimer.Start()
            myGrid.saveInitial()
        Else
            ' Objective 4. b.
            ButtonRun.Text = "RUN"
            myTimer.Enabled = False
            myTimer.Stop()
        End If
    End Sub

    ' Objective 4. c.
    Private Sub ButtonStep_Click(sender As Object, e As EventArgs)
        myTimer.Enabled = False
        myTimer.Stop()
        myGrid.updateGridToNextState()
        generation += 1
        updateInfo()
        myGrid.Invalidate()
    End Sub

    ' Objective 4. d.
    Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
        myTimer.Enabled = False
        myTimer.Stop()
        If myGrid.isSelect Then
            For y = myGrid.startCellY To myGrid.endCellY
                For x = myGrid.startCellX To myGrid.endCellX
                    myGrid.myCell(x, y).currentState = False
                Next
            Next
        Else
            myGrid.clearGrid()
            Me.generation = 0
        End If
        updateInfo()
        myGrid.Invalidate()
    End Sub

    ' Objective 4. e.
    Private Sub ButtonRewind_Click(sender As Object, e As EventArgs)
        myGrid.clearGrid()
        myGrid.returnInitial()
        myGrid.Invalidate()
        Me.generation = 0
        updateInfo()
    End Sub

    ' Objective 4. f.
    Private Sub ButtonUndo_Click(sender As Object, e As EventArgs)
        If myGrid.myCell(0, 0).undoState.Count > 0 Then
            myGrid.undoGrid()
            myGrid.Invalidate()
            generation -= 1
            updateInfo()
        Else
            MessageBox.Show("You cannot undo.")
        End If
    End Sub

    ' Objective 4. g.
    Private Sub ButtonSkip_Click(sender As Object, e As EventArgs)
        Dim num As String
        num = Trim(InputBox("Please enter the number of generations you would like to skip. "))
        If Not IsNumeric(num) Then
            MessageBox.Show("Please enter numbers only.")
        ElseIf Not num = Int(num) Then
            MessageBox.Show("Please enter integers only.")
        ElseIf num <= 0 Then
            MessageBox.Show("Please enter numbers greater than 0.")
        Else
            For n = 1 To CInt(num)
                myGrid.updateGridToNextState()
            Next
            myGrid.Invalidate()
            generation += CInt(num)
            updateInfo()
        End If
    End Sub


    Private Sub ButtonRandom_Click(sender As Object, e As EventArgs)
        randomisePattern()
        myGrid.Invalidate()
    End Sub

    ' Objective 4. h., 5. c.
    Private Sub speedTrackbar_Scroll(sender As Object, e As EventArgs)
        speedLabel.Text = String.Format("Time interval: {0} ms", speedTrackbar.Value)
        myTimer.Interval = speedTrackbar.Value
    End Sub

    ' Objective 4. i.
    Private Sub scaleTrackbar_Scroll(sender As Object, e As EventArgs)
        myGrid.changeScale(scaleTrackbar.Value)
        myGrid.Invalidate()
    End Sub

    ' Objective 6. b.
    Private Sub borderWidthTrackbar_Scroll(sender As Object, e As EventArgs)
        myGrid.changeWidth(borderWidthTrackbar.Value)
        myGrid.Invalidate()

    End Sub

    ' Objective 6. a.
    Private Sub ButtonCellColour_Click(sender As Object, e As EventArgs)
        Dim MyDialog As New ColorDialog()

        If (MyDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            myGrid.changeCellColour(MyDialog.Color)
            myGrid.Invalidate()
        End If
    End Sub

    Private Sub ButtonDeadCellColour_Click(sender As Object, e As EventArgs)
        Dim MyDialog As New ColorDialog()

        If (MyDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            myGrid.changeDeadCellColour(MyDialog.Color)
            myGrid.Invalidate()
        End If
    End Sub

    Private Sub ButtonBorderColour_Click(sender As Object, e As EventArgs)
        Dim MyDialog As New ColorDialog()

        If (MyDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            myGrid.changeBorderColour(MyDialog.Color)
            myGrid.Invalidate()
        End If
    End Sub

    Private Sub ButtonExport_Click(sender As Object, e As EventArgs)
        Export()
    End Sub

    Private Sub ButtonImport_Click(sender As Object, e As EventArgs)
        Import()
    End Sub

    Private Sub ButtonCopy_Click(sender As Object, e As EventArgs)
        If myGrid.isSelect = True Then
            myForm4 = New EditingForm(myGrid)
            myForm4.Show()
            ButtonSelect.CheckState = False
            selected = True
        Else
            MessageBox.Show("You cannot copy without selecting.")
        End If
    End Sub

    Private Sub ButtonPaste_Click(sender As Object, e As EventArgs)
        ' Objective 2. f. iv.
        If selectedCells Is Nothing Then
            MessageBox.Show("You cannot paste without copying.")
        ElseIf myGrid.isSelect = True Then
            Dim i As Integer = myGrid.startCellX
            Dim j As Integer = myGrid.startCellY
            For y = 0 To selectedCells.GetLength(1) - 1
                For x = 0 To selectedCells.GetLength(0) - 1
                    myGrid.myCell(i, j).currentState = selectedCells(x, y)
                    i += 1
                Next
                i = myGrid.startCellX
                j += 1
            Next
            myGrid.Invalidate()
        Else
            MessageBox.Show("You cannot paste without selecting.")
        End If
    End Sub

    ' Objective 3. d.
    Private Sub liveTextBox_TextChanged(sender As Object, e As EventArgs)
        myGrid.liveNum = liveTextBox.Text

    End Sub

    Private Sub bornTextBox_TextChanged(sender As Object, e As EventArgs)
        myGrid.bornNum = bornTextBox.Text
    End Sub

    Private Sub conwayLabel_Click(sender As Object, e As EventArgs)
        Me.liveTextBox.Text = conwayLiveNum
        Me.bornTextBox.Text = conwayBornNum
        myGrid.liveNum = conwayLiveNum
        myGrid.bornNum = conwayBornNum
    End Sub

    Private Sub replicatorLabel_Click(sender As Object, e As EventArgs)
        Me.liveTextBox.Text = replicatorLiveNum
        Me.bornTextBox.Text = replicatorBornNum
        myGrid.liveNum = replicatorLiveNum
        myGrid.bornNum = replicatorBornNum
    End Sub

    Private Sub mazeLabel_Click(sender As Object, e As EventArgs)
        Me.liveTextBox.Text = mazeLiveNum
        Me.bornTextBox.Text = mazeBornNum
        myGrid.liveNum = mazeLiveNum
        myGrid.bornNum = mazeBornNum
    End Sub

    Private Sub twoLabel_Click(sender As Object, e As EventArgs)
        Me.liveTextBox.Text = twoLiveNum
        Me.bornTextBox.Text = twoBornNum
        myGrid.liveNum = twoLiveNum
        myGrid.bornNum = twoBornNum
    End Sub

    Private Sub HLLabel_Click(sender As Object, e As EventArgs)
        Me.liveTextBox.Text = HLLiveNum
        Me.bornTextBox.Text = HLBornNum
        myGrid.liveNum = HLLiveNum
        myGrid.bornNum = HLBornNum
    End Sub

    ' Objective 3. a.
    Private Sub comboBoxEdge_SelectedIndexChanged(sender As Object, e As EventArgs)

        'MessageBox.Show("The boundary condition has changed to " & sender.selecteditem)
        myGrid.boundaryCondition = sender.selectedItem
    End Sub

    Private Sub ButtonBack_Click(sender As Object, e As EventArgs)
        Me.Hide()
        MenuForm.Show()
    End Sub

    Private Sub ButtonAbout_Click(sender As Object, e As EventArgs)
        Dim rules As New UserGuideForm
        rules.Show() ' Open the rule form
    End Sub

    ' Objective 5. a., 5. b.
    Private Sub updateInfo()
        infoLabel.Text = String.Format("|  Generation: {0}  |  Population: {1}  |", generation, myGrid.population)
    End Sub

    ' Objective 2. b.
    Public Sub randomisePattern()
        Randomize()
        For y = 0 To myGrid.gridSize - 1
            For x = 0 To myGrid.gridSize - 1
                myGrid.myCell(x, y).currentState = Int(Rnd() * 2)
            Next
        Next
    End Sub

    ' Objective 2. c., 2. d.
    Public Sub Import()
        Dim fileReader As System.IO.StreamReader
        Dim openFile As OpenFileDialog = New OpenFileDialog()
        Dim fileName As String = ""

        openFile.Title = "Import"
        openFile.InitialDirectory = Application.StartupPath() & "\Patterns"

        If openFile.ShowDialog() = DialogResult.OK Then
            fileName = openFile.FileName
        End If

        Try
            fileReader = New IO.StreamReader(fileName)
            Dim stringReader As String
            Dim indexList As New List(Of String())

            Dim myArray(1) As String
            Dim x As Integer
            Dim y As Integer
            Dim maxX As Integer = 0
            Dim maxY As Integer = 0

            Do
                stringReader = fileReader.ReadLine()
                myArray = Split(stringReader)

                x = CInt(myArray(0))
                y = CInt(myArray(1))
                maxX = Math.Max(maxX, x + 1)
                maxY = Math.Max(maxY, y + 1)
                indexList.Add(myArray)
            Loop Until fileReader.EndOfStream

            Dim startX As Integer = Int((gridSize - maxX) / 2)
            Dim startY As Integer = Int((gridSize - maxY) / 2)

            For n = 0 To indexList.Count - 1
                myArray = indexList(n)
                x = myArray(0) + startX
                y = myArray(1) + startY
                myGrid.myCell(x, y).currentState = True
            Next

            fileReader.Close()
            Me.Invalidate()
        Catch ex As Exception
            MessageBox.Show("Failed to import file.")
        End Try
    End Sub

    ' Objective 2. e.
    Public Sub Export()
        Dim fileWriter As System.IO.StreamWriter
        Dim fileName As String = ""
        fileName = Trim(InputBox("Enter the file name here"))

        If fileName = "" Then
            MessageBox.Show("You must input a file name. Please try again.")
        ElseIf System.IO.File.Exists(Application.StartupPath() & "\Patterns\" & fileName & ".txt") Then
            MessageBox.Show("File name already exists. Please try again.")
        Else
            fileWriter = New IO.StreamWriter(Application.StartupPath() & "\Patterns\" & fileName & ".txt")

            Dim minX As Integer = 350
            Dim minY As Integer = 350
            For y = 0 To myGrid.gridSize - 1
                For x = 0 To myGrid.gridSize - 1
                    If myGrid.myCell(x, y).currentState Then
                        minX = Math.Min(minX, x)
                        minY = Math.Min(minY, y)
                    End If
                Next
            Next

            For y = 0 To myGrid.gridSize - 1
                For x = 0 To myGrid.gridSize - 1
                    If myGrid.myCell(x, y).currentState Then
                        fileWriter.WriteLine(x - minX & " " & y - minY)
                    End If
                Next
            Next

            fileWriter.Close()
            MessageBox.Show("Export successfully!")
        End If
    End Sub

End Class
