Public Class Grid
    Inherits PictureBox
    Protected _gridX As Integer
    Protected _gridY As Integer
    Protected _myCell(,) As Cell
    Protected _cellSize As Integer
    Protected _borderWidth As Integer = 1
    Protected _borderColour As Color = Color.Gray

    Public Sub New()

    End Sub

    Public Overridable Sub createBoard(ByVal gridX As Integer, ByVal gridY As Integer, ByVal w As Integer, ByVal h As Integer, ByVal cellSize As Integer)
        _cellSize = cellSize
        renew(gridX, gridY)

        Me.Top = (h - Me.Height) \ 2
        Me.Left = (w - Me.Width) \ 2
        Me.BackColor = _borderColour

    End Sub

    ' Initialise or reset the size of the grid
    Public Sub renew(ByVal gridX As Integer, ByVal gridY As Integer)
        _gridX = gridX
        _gridY = gridY
        ReDim _myCell(_gridX - 1, _gridY - 1)
        For y = 0 To _gridY - 1
            For x = 0 To _gridX - 1
                _myCell(x, y) = New Cell(_cellSize, _borderWidth, x, y)
            Next
        Next

        Me.Height = _gridY * _cellSize
        Me.Width = _gridX * _cellSize
    End Sub

    Public Sub displayGrid(myGraphics As Graphics)
        ' Draw each cell one by one using a nested for loops
        For y = 0 To _gridY - 1
            For x = 0 To _gridX - 1
                _myCell(x, y).drawCell(myGraphics)
            Next
        Next
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        ' Paints the grid
        displayGrid(e.Graphics)
    End Sub

    Public Property myCell(x As Integer, y As Integer) As Cell
        Get
            Return _myCell(x, y)
        End Get
        Set(value As Cell)
            _myCell(x, y) = value
        End Set
    End Property

    Public Property cellSize As Integer
        Get
            Return _cellSize
        End Get
        Set(value As Integer)
            _cellSize = value
        End Set
    End Property


End Class


