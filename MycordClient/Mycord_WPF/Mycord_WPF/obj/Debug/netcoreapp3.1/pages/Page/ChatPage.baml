   M S B A M L   `   `   `  ����E  AMycord_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Mycord_WPF
Mycord_WPF  5          Mycord_WPF.ChatPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      *localclr-namespace:Mycord_WPF     5        N� Title$  ChatPage��5
      .��5      �� ._�5   
   �    BackgroundDiscordBlackBrush#��[  6    � ColumnDefinitions 5      �� 5      $	��250 �6#   �� 5      $��* �6#   .+�5      � $��0��6      BackgroundDiscordBlack2Brush#��[ 6#   .+�5      �� $
��Left=�6   $��10q�68   .+�5      �  5      �� 5      $	��200 �6/   �� 5      $��30 �6/   .+�5      ��   	MapleBold#��[ 6>     ForegroundDiscordMainBrush#��[ 6f   $��30(�6�   $��10q�6�   ��6$   �� *
RoomName+6�   �� 5      $��1��6!   $	�� % ��61   $��Center=�6_   $��Center=�6z   ��6?   �� *ExitCommand+6z   5      �� 5      $��Hidden=�6#   .��5      �� #��[ 6H   $��0q�6�   $|�False��6�   ��6'   �� *
UserList+��5      �� 5   "   .R�5    &   ��  ��Name$ border��6T    �� Padding$ 	10 0 10 0q�6-   $��	0 10 0 10q�6A   #��[ 6d   ._�5!   *   � .+�5"   .   �� $��1��65   $ 10 0 0 0q�6E   ._�5#   2   �� $
��Wrap=�6<   $��Center=�6f   $
��Left=�6�   $��20(�6�   #��[ 6�   #��[ 6�   ��6P   �� *Name+6�   5$   /   5%   +   5&   '   5'   #   5)      5*      5,      5-      � 5.      $��1��6    � RowDefinitions 5/      �� 50      $��* �6$   �� 51      $
��Auto �6$   .+�53      �� #   Mycord_WPF.AutoScrollProperty   AlwaysScrollToEnd$
 True��6   #��[ 6Q   $|�False��6�   .��54      �� #��[  6D   $��0q�6~   $|�False��6�   ��6#   �� *
ChatList+��55      �� 56      .R�57   "   �� $ border��6P   $ 	10 0 10 0q�6)   $��	0 10 0 10q�6=   #��[  6`    � Triggers 58   &   :� 59   *    :� RoutedEvent$ 
MouseEnter��67   .[�5:   .   �� .��5;   2   ��  �� 
TargetName$ border��6=   	 �� TargetProperty$	 Background.Color�6\   .��5<   6   ��   BackgroundDiscordVeryBlack	
 �� To#
 [ 6E    g� Duration$ 0:0:0.2D�6v   5=   3   5>   /   5?   +   :� 5@   *   $ 
MouseLeave��67   .[�5A   .   �� .��5B   2   �� $ border��6=   $	 Background.Color�6\   .��5C   6   ��   BackgroundDiscordBlack#
 [ 6E   $ 0:0:0.2D�6r   5D   3   5E   /   5F   +   ._�5H   &   �  5I   *   �� 5J   .   $��50 �6?   �� 5K   .   $��* �6?   .+�5M   *   ��  �� CornerRadius$ 50��61     BackgroundDiscordBlueBrush#��[ 6C   $��50��6|   $	��Top=�6�   ._�5N   .   �� $
��Null��68   $��Center=�6D   $��Center=�6_   $��23(�6|   #��[ 6�   5O   +   �� 5P   *   $��1��61   $ 10 0 0 0q�6A   ._�5Q   .   �� .+�5R   2   ��  �� Orientation$ 
Horizontal=�6=   .+�5S   6   �� $
��Wrap=�6@   $��Center=�6j   $
��Left=�6�   $��22(�6�     
MapleLight#��[ 6�   #��[ 6�   ��6T   �� *Name+6�   �� 5T   6    ��Name$ TimeText��6R   $��10,0,0,0q�6@   $��Center=�6�   $��Right=�6�   $��19(�6�   #��[ 6�    $  ForegroundDiscordSecondMainBrush#��[ 6  ��6d   �� *
SendTime+ �� 	Converter $   Mycord_WPF.TimeFormatConverter  6  5U   3   �� 5V   2   $
��Wrap=�6<   $��Center=�6i   $
��Left=�6�   $��20(�6�   #��[ 6�   #��[ 6�   $��5,0,0,0q�6  ��6P   �� *	Message+6  5W   /   5X   +   5Y   '   5Z   #   5[      5]      5^      � 5a      $��1��6   $��60��6$   $��Bottom=�63   .+�5b      ��  %	 !BackgroundDiscordLittleBlackBrush#��[	 6   $ 8q�5c      $��
20 0 20 15q�6)   $ 10��5d      ._�5e      �� -   5         ��Name$ SendBox��5e   "    
 TextBoxPlaceHolder#��[
 5f      
 � Tag$ 메세지 보내기��5g   "   " �� VerticalScrollBarVisibility$
 Auto=�5h   "   $��Center=�5i   "   $��3q�6D   #��[ 5j   "   #��[ 5k   "   
������ $��0q�5l   ;   $��18(�6O   ��5m      �� *TextBoxText+ �� UpdateSourceTrigger$ PropertyChanged=�.��6`    J� InputBindings 5n      �� 5o   "   
 �� Key$ Return��6�    �� Command 6-   �� *MessageSendCommand+ �� CommandParameter 6T   �� *Text+ �� ElementName$ SendBox��6�   .��5p   5   5q      5r      5s      5t      5u      5v      5x      