Public Class EditingForm
    Private selectGrid As GOLGrid ' Stores the grid from the simulator
    Private selectBoard As Grid ' Stores the selected 
    Private temp(,) As Boolean ' Stores the result of editing temporarily
    Private lengthX As Integer ' Stores the number of columns selected
    Private lengthY As Integer ' Stores the number of rows selected
    Private tempX As Integer
    Private tempY As Integer

    Dim ButtonRotateC As Button
    Dim ButtonRotateAC As Button
    Dim ButtonFlipH As Button
    Dim ButtonFlipV As Button
    Dim ButtonDone As Button

    Const maxLength As Integer = 540 ' Stores the maximum number of pixels for the grid to display

    Public Sub New(ByVal myGrid As GOLGrid)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        selectGrid = myGrid

        ' Store the length and width of the selected region
        lengthX = selectGrid.endCellX - selectGrid.startCellX + 1
        lengthY = selectGrid.endCellY - selectGrid.startCellY + 1

        ' Objective 2. f. ii.
        selectBoard = New Grid()
        Dim n As Integer = Math.Min(maxLength \ lengthX, maxLength \ lengthY)
        selectBoard.createBoard(lengthX, lengthY, maxLength, maxLength, n)

        ReDim temp(lengthX - 1, lengthY - 1)

        Dim startX As Integer = selectGrid.startCellX ' Stores the column number of the top-left cell of the selected region
        Dim startY As Integer = selectGrid.startCellY ' Stores the row number of the top-left cell of the selected region
        Dim endX As Integer = selectGrid.endCellX ' Stores the column number of the bottom-right cell of the selected region
        Dim endY As Integer = selectGrid.endCellY ' Stores the row number of the bottom-right cell of the selected region

        ' Stores the state of cells of the selected region
        Dim i As Integer = 0
        Dim j As Integer = 0
        For y = startY To endY
            For x = startX To endX
                selectBoard.myCell(i, j).currentState = selectGrid.myCell(x, y).currentState
                i += 1
            Next
            i = 0
            j += 1
        Next
    End Sub

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.ControlBox = False
        Me.ClientSize = New Size(700, 580)
        Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        Me.Text = "Editing"

        selectBoard.Top += 20
        selectBoard.Left += 20
        Me.Controls.Add(selectBoard)

        'Create Rotate Clockwise Button
        ButtonRotateC = New Button
        ButtonRotateC.BackColor = Color.White
        ButtonRotateC.BackgroundImage = Image.FromFile("Icon\rotate clockwise.png")
        ButtonRotateC.BackgroundImageLayout = ImageLayout.Stretch
        ButtonRotateC.Height = 30
        ButtonRotateC.Width = 30
        ButtonRotateC.Location = New Point(600, 50)
        AddHandler ButtonRotateC.Click, AddressOf ButtonRotateC_Click
        Me.Controls.Add(ButtonRotateC)

        'Create Rotate Anti-Clockwise Button
        ButtonRotateAC = New Button
        ButtonRotateAC.BackColor = Color.White
        ButtonRotateAC.BackgroundImage = Image.FromFile("Icon\rotate anticlockwise.png")
        ButtonRotateAC.BackgroundImageLayout = ImageLayout.Stretch
        ButtonRotateAC.Height = 30
        ButtonRotateAC.Width = 30
        ButtonRotateAC.Location = New Point(600, 100)
        AddHandler ButtonRotateAC.Click, AddressOf ButtonRotateAC_Click
        Me.Controls.Add(ButtonRotateAC)

        'Create Flip Horizontal Button
        ButtonFlipH = New Button
        ButtonFlipH.BackColor = Color.White
        ButtonFlipH.BackgroundImage = Image.FromFile("Icon\flip horizontal.png")
        ButtonFlipH.BackgroundImageLayout = ImageLayout.Stretch
        ButtonFlipH.Height = 30
        ButtonFlipH.Width = 30
        ButtonFlipH.Location = New Point(600, 150)
        AddHandler ButtonFlipH.Click, AddressOf ButtonFlipH_Click
        Me.Controls.Add(ButtonFlipH)

        'Create Flip Vertical Button
        ButtonFlipV = New Button
        ButtonFlipV.BackColor = Color.White
        ButtonFlipV.BackgroundImage = Image.FromFile("Icon\flip vertical.png")
        ButtonFlipV.BackgroundImageLayout = ImageLayout.Stretch
        ButtonFlipV.Height = 30
        ButtonFlipV.Width = 30
        ButtonFlipV.Location = New Point(600, 200)
        AddHandler ButtonFlipV.Click, AddressOf ButtonFlipV_Click
        Me.Controls.Add(ButtonFlipV)

        'Create Done Button
        ButtonDone = New Button
        ButtonDone.Text = "Done"
        ButtonDone.Height = 40
        ButtonDone.Width = 60
        ButtonDone.Location = New Point(580, 300)
        ButtonDone.Font = New Font("courier new", 12)
        AddHandler ButtonDone.Click, AddressOf ButtonDone_Click
        Me.Controls.Add(ButtonDone)
    End Sub

    ' Objective 2. f. iii.
    ' Rotate the pattern clockwise
    Private Sub RotateC()
        tempX = lengthY
        tempY = lengthX
        lengthX = tempX
        lengthY = tempY

        ReDim temp(lengthX - 1, lengthY - 1)

        For x = 0 To lengthX - 1
            For y = 0 To lengthY - 1
                temp(lengthX - 1 - x, y) = selectBoard.myCell(y, x).currentState
            Next
        Next

        selectBoard.renew(lengthX, lengthY)

        updateResult()
    End Sub

    ' Rotate the pattern anti-clockwise
    Private Sub RotateAC()
        tempX = lengthY
        tempY = lengthX
        lengthX = tempX
        lengthY = tempY

        ReDim temp(lengthX - 1, lengthY - 1)

        For y = 0 To lengthY - 1
            For x = 0 To lengthX - 1
                temp(x, y) = selectBoard.myCell(lengthY - 1 - y, x).currentState
            Next
        Next

        selectBoard.renew(lengthX, lengthY)

        updateResult()
    End Sub

    ' Flip the pattern horizontally
    Private Sub flipH()
        For y = 0 To lengthY - 1
            For x = 0 To lengthX - 1
                temp(x, y) = selectBoard.myCell(lengthX - 1 - x, y).currentState
            Next
        Next

        updateResult()
    End Sub

    ' Flip the pattern vertically
    Private Sub flipV()
        For y = 0 To lengthY - 1
            For x = 0 To lengthX - 1
                temp(x, y) = selectBoard.myCell(x, lengthY - 1 - y).currentState
            Next
        Next

        updateResult()
    End Sub

    Private Sub ButtonRotateC_Click(sender As Object, e As EventArgs)
        RotateC()
    End Sub

    Private Sub ButtonRotateAC_Click(sender As Object, e As EventArgs)
        RotateAC()
    End Sub

    Private Sub ButtonFlipH_Click(sender As Object, e As EventArgs)
        flipH()
    End Sub

    Private Sub ButtonFlipV_Click(sender As Object, e As EventArgs)
        flipV()
    End Sub

    Private Sub ButtonDone_Click(sender As Object, e As EventArgs)

        ' Pass the edited pattern to the simulator
        ReDim MenuForm.main.selectedCells(lengthX, lengthY)
        For y = 0 To lengthY - 1
            For x = 0 To lengthX - 1
                MenuForm.main.selectedCells(x, y) = selectBoard.myCell(x, y).currentState
            Next
        Next
        Me.Close()

    End Sub

    ' Redraw the grid after editing
    Private Sub updateResult()
        For y = 0 To lengthY - 1
            For x = 0 To lengthX - 1
                selectBoard.myCell(x, y).currentState = temp(x, y)
            Next
        Next
        selectBoard.Top = (maxLength - selectBoard.Height) \ 2 + 20
        selectBoard.Left = (maxLength - selectBoard.Width) \ 2 + 20
        selectBoard.Invalidate()
    End Sub

End Class