Public Class GOLGrid
    Inherits Grid

    Private _gridSize As Integer ' The number of columns/ rows
    Public Property gridSize As Integer
        Get
            Return _gridSize
        End Get
        Set(value As Integer)
            _gridSize = value
        End Set
    End Property

    Private _gridLength As Integer ' The number of pixels displayed

    Private _boundaryCondition As String ' Stores the slected boundary condition
    Public Property boundaryCondition() As String
        Get
            Return _boundaryCondition
        End Get
        Set(ByVal value As String)
            _boundaryCondition = value
        End Set
    End Property


    Private _clickedCell As Rectangle ' Stores the region of the clicked cell
    Public Property clickedCell As Rectangle
        Get
            Return _clickedCell
        End Get
        Set(value As Rectangle)
            _clickedCell = value
        End Set
    End Property

    ' Variables to differentiate click, drag and select
    Private _isClicked As Boolean = True
    Private _isDragging As Boolean = False
    Private _isSelect As Boolean = False
    Public Property isSelect As Boolean
        Get
            Return _isSelect
        End Get
        Set(value As Boolean)
            _isSelect = value
        End Set
    End Property

    Private _startPoint As Point ' Stores the point of mouse down

    ' Stores the column number of the top-left cell of the selected region
    Private _startCellX As Integer = 0
    Public Property startCellX As Integer
        Get
            Return _startCellX
        End Get
        Set(value As Integer)
            _startCellX = value
        End Set
    End Property

    ' Stores the row number of the top-left cell of the selected region
    Private _startCellY As Integer = 0
    Public Property startCellY As Integer
        Get
            Return _startCellY
        End Get
        Set(value As Integer)
            _startCellY = value
        End Set
    End Property

    ' Stores the column number of the bottom-right cell of the selected region
    Private _endCellX As Integer = 0
    Public Property endCellX As Integer
        Get
            Return _endCellX
        End Get
        Set(value As Integer)
            _endCellX = value
        End Set
    End Property

    ' Stores the row number of the bottom-right cell of the selected region
    Private _endCellY As Integer = 0
    Public Property endCellY As Integer
        Get
            Return _endCellY
        End Get
        Set(value As Integer)
            _endCellY = value
        End Set
    End Property

    ' The number of list that stores the number of neighbours required for a cell to survive
    Private _liveNum As String = "23"
    Public Property liveNum As String
        Get
            Return _liveNum
        End Get
        Set(value As String)
            _liveNum = value
        End Set
    End Property

    'The number of list that stores the number of neighbours required for a cell to born
    Private _bornNum As String = "3"
    Public Property bornNum As String
        Get
            Return _bornNum
        End Get
        Set(value As String)
            _bornNum = value
        End Set
    End Property

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Sub createBoard(gridX As Integer, gridY As Integer, w As Integer, h As Integer, ByVal cellSize As Integer)
        MyBase.createBoard(gridX, gridY, w, h, cellSize)
        _gridSize = gridX
        _gridLength = h
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        SelectCells(e) ' Paint the highlighted region
    End Sub

    Public Sub changeScale(ByVal size As Integer)
        _cellSize = size
        Me.Height = _gridSize * _cellSize
        Me.Width = _gridSize * _cellSize
        Me.Top = -(Me.Height - _gridLength) / 2
        Me.Left = -(Me.Width - _gridLength) / 2
        For y = 0 To _gridSize - 1
            For x = 0 To _gridSize - 1
                _myCell(x, y).posX = x * size
                _myCell(x, y).posY = y * size
                _myCell(x, y).size = size
            Next
        Next
    End Sub

    Public Sub changeWidth(ByVal width As Integer)
        _borderWidth = width
        For y = 0 To _gridSize - 1
            For x = 0 To _gridSize - 1
                _myCell(x, y).borderWidth = width
            Next
        Next
    End Sub

    Public Sub changeCellColour(ByVal colour As Color)
        For y = 0 To _gridSize - 1
            For x = 0 To _gridSize - 1
                _myCell(x, y).cellColour = colour
            Next
        Next
    End Sub

    Public Sub changeDeadCellColour(ByVal colour As Color)
        For y = 0 To _gridSize - 1
            For x = 0 To _gridSize - 1
                _myCell(x, y).deadCellColour = colour
            Next
        Next
    End Sub

    Public Sub changeBorderColour(ByVal colour As Color)
        Me.BackColor = colour
    End Sub


    Public Function population() As Integer 'The toatl number of living cells
        Dim pop As Integer
        For y = 0 To _gridSize - 1
            For x = 0 To _gridSize - 1
                If _myCell(x, y).currentState Then
                    pop += 1
                End If
            Next
        Next
        Return pop
    End Function

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)
        If _isClicked And Not isSelect Then
            Call changeStateOnMouseClick(e.X, e.Y)
            Me.Invalidate(_clickedCell) ' Only invalidate the selected region to reduce delays
        End If
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        _isDragging = True
        _startPoint = New Point(e.X, e.Y)
        _startCellX = Int(e.X / _cellSize)
        _startCellY = Int(e.Y / _cellSize)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If _isDragging Then
            _isClicked = False
            If isSelect Then
                _endCellX = Int(e.X / _cellSize)
                _endCellY = Int(e.Y / _cellSize)
                Me.Invalidate()
            Else
                ' Objective 1. c.
                'Move the gird when drag
                Dim myTop As Integer = Me.Top - _startPoint.Y + e.Y
                Dim myLeft As Integer = Me.Left - _startPoint.X + e.X
                If myTop <= 0 And myTop >= -Me.Height + _gridLength Then
                    Me.Top = myTop
                End If
                If myLeft <= 0 And myLeft >= -Me.Width + _gridLength Then
                    Me.Left = myLeft
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        _isDragging = False
        _isClicked = True
        _endCellX = Int(e.X / _cellSize)
        _endCellY = Int(e.Y / _cellSize)
        Me.Invalidate()
    End Sub

    ' Objective 2. f. i.
    Public Sub SelectCells(ByVal e As PaintEventArgs)
        If isSelect Then
            Dim startx As Integer = _startCellX * _cellSize
            Dim starty As Integer = _startCellY * _cellSize
            Dim w As Integer = (_endCellX - _startCellX + 1) * _cellSize
            Dim h As Integer = (_endCellY - _startCellY + 1) * _cellSize

            ' Highlight the selected region
            Dim brushColour As Color = Color.Black
            Using myPen As Pen = New Pen(Color.Blue, 3)
                e.Graphics.DrawRectangle(myPen, startx, starty, w, h)
            End Using
            Using myBrush As SolidBrush = New SolidBrush((Color.FromArgb(100, Color.LightBlue)))
                e.Graphics.FillRectangle(myBrush, startx, starty, w, h)
            End Using
        End If
    End Sub

    Public Sub clearGrid()
        For y = 0 To _gridSize - 1
            For x = 0 To _gridSize - 1
                _myCell(x, y).currentState = False
                _myCell(x, y).nextState = False
                _myCell(x, y).undoState.Clear()
            Next
        Next
    End Sub

    Public Sub undoGrid()
        For y = 0 To _gridSize - 1
            For x = 0 To _gridSize - 1
                _myCell(x, y).currentState = _myCell(x, y).undoState.Pop()
                _myCell(x, y).nextState = False
            Next
        Next
    End Sub

    Public Sub saveInitial()
        For y = 0 To _gridSize - 1
            For x = 0 To _gridSize - 1
                _myCell(x, y).initialState = _myCell(x, y).currentState
            Next
        Next
    End Sub

    Public Sub returnInitial()
        For y = 0 To _gridSize - 1
            For x = 0 To _gridSize - 1
                _myCell(x, y).currentState = _myCell(x, y).initialState
            Next
        Next
    End Sub

    ' Objective 2. a.
    Public Sub changeStateOnMouseClick(ByVal x As Integer, ByVal y As Integer)
        x = Int(x / _cellSize) ' The x index of the cell
        y = Int(y / _cellSize) ' The y index of the cell
        _clickedCell = New Rectangle(x * _cellSize, y * _cellSize, _cellSize, _cellSize) 'The area of the clicked cell
        _myCell(x, y).changeState()
    End Sub

    ' Objective 3. c.
    Public Sub updateGridToNextState()
        Dim count As Integer = 0
        For y = 0 To _gridSize - 1
            For x = 0 To _gridSize - 1

                If _boundaryCondition = "Closed Boundaries" Then
                    count = countLiveNeighbours(x, y)
                ElseIf _boundaryCondition = "Toroidal Boundaries" Then
                    count = countLiveNeighboursWrap(x, y)
                End If

                If _myCell(x, y).currentState = True Then
                    If Not _liveNum.Contains(count) Then
                        _myCell(x, y).nextState = False
                    Else
                        _myCell(x, y).nextState = True
                    End If
                Else
                    If _bornNum.Contains(count) Then
                        _myCell(x, y).nextState = True
                    Else
                        _myCell(x, y).nextState = False
                    End If
                End If
            Next
        Next

        For y = 0 To _gridSize - 1
            For x = 0 To _gridSize - 1
                _myCell(x, y).undoState.Push(_myCell(x, y).currentState)
                _myCell(x, y).currentState = _myCell(x, y).nextState
            Next
        Next
    End Sub

    ' Objective 3. b.
    Public Function countLiveNeighbours(ByVal x As Integer, ByVal y As Integer) As Integer
        Dim count As Integer = 0

        ' Check if the cell is located at the top left
        If x <> 0 And y <> 0 Then
            If _myCell(x - 1, y - 1).currentState = True Then
                count += 1
            End If
        End If

        ' Check if the cell is located at the top middle
        If y <> 0 Then
            If _myCell(x, y - 1).currentState = True Then
                count += 1
            End If
        End If

        ' Check if the cell is located at the top right
        If x <> _gridSize - 1 And y <> 0 Then
            If _myCell(x + 1, y - 1).currentState = True Then
                count += 1
            End If
        End If

        ' Check if the cell is located at the left
        If x <> 0 Then
            If _myCell(x - 1, y).currentState = True Then
                count += 1
            End If
        End If

        ' Check if the cell is located at the right
        If x <> _gridSize - 1 Then
            If _myCell(x + 1, y).currentState = True Then
                count += 1
            End If
        End If

        ' Check if the cell is located at the bottom left
        If x <> 0 And y <> _gridSize - 1 Then
            If _myCell(x - 1, y + 1).currentState = True Then
                count += 1
            End If
        End If

        ' Check if the cell is located at the bottom middle
        If y <> _gridSize - 1 Then
            If _myCell(x, y + 1).currentState = True Then
                count += 1
            End If
        End If

        ' Check if the cell is located at the bottom right
        If x <> _gridSize - 1 And y <> _gridSize - 1 Then
            If _myCell(x + 1, y + 1).currentState = True Then
                count += 1
            End If
        End If

        Return count
    End Function

    Public Function countLiveNeighboursWrap(ByVal x As Integer, ByVal y As Integer) As Integer
        Dim count As Integer = 0

        For j = 0 To 2
            For i = 0 To 2
                If _myCell((x - 1 + _gridSize + i) Mod _gridSize, (y - 1 + _gridSize + j) Mod _gridSize).currentState Then
                    count += 1
                End If
            Next
        Next

        If _myCell(x, y).currentState Then
            count -= 1
        End If

        Return count
    End Function

End Class
