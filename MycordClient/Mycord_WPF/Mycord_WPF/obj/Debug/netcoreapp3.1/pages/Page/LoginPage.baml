   M S B A M L   `   `   `  ����E  AMycord_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Mycord_WPF
Mycord_WPF  5          Mycord_WPF.LoginPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      *localclr-namespace:Mycord_WPF     5        N� Title$  	LoginPage��5
      .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ LoginBorder��5      $��1��5      $��1��6      BackgroundDiscordBlackBrush#��[  5   &   $	��350��6`   $	��600��6p    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 1Q�6#      Opacity$	 0.2Q�63   ._�5      �  5      �� 5       $��6* �6'   �� 5!      $��4* �6'   .+�5#      �� $��0��6   ._�5$      � $��Center=�6   $��Center=�68    5%      �� 5&      $
��Auto �6,   �� 5'      $
��Auto �6,   �� 5(      $
��Auto �6,   �� 5)      $
��Auto �6,   �� 5*      $
��Auto �6,   .+�5,      �� $��0��6%    �� TextAlignment$ Center=�62   $
��0 10q�6S   .+�5-      �� $-��'당신의 이름을 입력해주세요!��6(     ForegroundDiscordMainBrush#��[ 5.   $     	MapleBold#��[ 6]   $
��20 4q�6�   $��21(�6�   �� 5/      $+��%만나게 되서 너무 반가워요!��6(    $  ForegroundDiscordSecondMainBrush#��[ 50   $     
MapleLight#��[ 6c   $��15(�6�   51      �� 53      $��1��6%   $
��10 0q�62   .+�54      �� $��	닉네임��6(   #��[ 63   #��[ 6r   $��12(�6�   6�   �� 55      -   5         ��Name$ NameBox��55   &     TextBoxHolder#��[ 67   6`   56      �� 58      
���  ��#��[ 6P   $��2��6y   $
��10 0q�6�   ��6$   �� *	ErrorText+6�   �� 5:      $��3��6!     DiscordBlueButton#��[ 6.   $��	로그인��5=   !   $��10 13q�6/   "   Mycord_WPF.IsRunningProperty	   Value	 5;   !   �� *LoginIsRunning+{�6Z   �� *
Connection+��5<   !   �� *LoginCommand+��6B   �� *Text+
 �� ElementName$
 NameBox��5=   /   �� 5?      $��4��6!     
TextButton#��[ 6.   $1��+이용 문의는 여기를 눌러주세요.��5@   !   $
��Left=�6=     BackgroundDiscordBlueBrush#��[ 5A   !   $
��10 0q�6Z   5B      5C      �� 5D      $��1��6   ._�5E      � $��Center=�6   $��Center=�68    5F      �� 5G      $
��Auto �6,   �� 5H      $
��Auto �6,   �� 5I      $
��Auto �6,   .+�5K      (� $��0��6"   $	��200��6/   $��0,0,30,0q�6?   .��5L      �� $	����6(   #��[ 68    	 FontAwesome#��[	 6q   5M      (� 5N      $��1��6"   $��50��6/   $
��Left=�6>   .��5O      �� $��Made by ��6(   #��[ 68   #��[ 6w   5P      (� 5Q      $��2��6"   $��40��6/   $��Right=�6>   $��0,0,20,0q�6Z   .��5R      �� $��	YuJae Lee��6(   #��[ 69   #��[ 6x   5S      5T      5U      5W      5X      5[      5\      