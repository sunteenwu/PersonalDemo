Public Class RectItem
    Implements INotifyPropertyChanged
    Private Sub NotifyPropertyChanged(Optional propertyName As String = "")
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Property X As Double
        Get
            Return m_X
        End Get
        Set
            m_X = Value
            NotifyPropertyChanged()
        End Set
    End Property
    Private m_X As Double
    Public Property Y As Double
        Get
            Return m_Y
        End Get
        Set
            m_Y = Value
            NotifyPropertyChanged()
        End Set
    End Property
    Private m_Y As Double
    Public Property Width As Double
        Get
            Return m_Width
        End Get
        Set
            m_Width = Value
            NotifyPropertyChanged()
        End Set
    End Property
    Private m_Width As Double
    Public Property Height As Double
        Get
            Return m_Height
        End Get
        Set
            m_Height = Value
            NotifyPropertyChanged()
        End Set
    End Property
    Private m_Height As Double
End Class
