   M S B A M L   `   `   `  ����E  AMycord_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Mycord_WPF
Mycord_WPF  5   	   "    Mycord_WPF.CustomMessageBox2   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Mycord_WPF     5   	     � WindowStyle$
  None=�5   	    � Topmost$
 True��5	   	    � 
ResizeMode$ NoResize=�5
   	    � AllowsTransparency$
 True��6    � Title$ server maker��5   	   $	��200��6   $	��350��6+   '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      CaptionHeight$ 25Q�6   .��5      ��  �� Padding 6   �� *OuterMarginSizeThickness+ �� FallbackValue$ 0��._�5   
   � .+�5      �� 	 �� CornerRadius$	 0��6      BackgroundDiscordBlackBrush#��[  6&   
 J� Effect
 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 1Q�6'      Opacity$	 0.2Q�67   5      � 5       � RowDefinitions 5      �� 5      $��25 �6$   �� 5      $��2* �6$   �� 5      $��1* �6$   .+�5      �� $��1��6   .+�5       �� $��	10 5 10 5q�6!   .+�5!      �� $��서버 이름��6$    $  ForegroundDiscordSecondMainBrush#��[ 61     
MapleLight#��[ 6p   $��12(�6�   6�   �� 5"        TextBoxHolder#��[ 6"   ��6I   �� *
ServerName+5$      �� 5%      $��2��6    
���  ��#��[ 6Y   $
��10 0q�6�   ��6-   �� *	ErrorText+6�   5&      �� 5*      $
��10 0q�6   $��3��6+    �� Orientation$ 
Horizontal=�68   $��Right=�6Q   .+�5+      ��   DiscordBlueButton#��[ 6   $��20 10q�5,   -   $	��100��6<   $��찾기��6H   $��1q�5-   -   $��14(�69     ForegroundDiscordMainBrush#��[ 6G   "   Mycord_WPF.IsRunningProperty   Value 5.   -   �� *IsSearching+��6c   �� *EnterRoomCommand+�� 5/        	Hoverless#��[ 6   $��10q�6a   ��6@   �� *CloseCommand+.��50      (� .��51      �� $	����6(   #��[ 68     FontAwesome#��[ 6w   52      53      54      55      56      57      58      