   M S B A M L   `   `   `  ����E  AMycord_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Mycord_WPF
Mycord_WPF  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source �� 
TargetType   TextBoxHolder  	BaseStyle �� BasedOn �� Property �� Value  ForegroundDarkBrush $  ForegroundDiscordSecondMainBrush  
MapleLight # BackgroundDiscordVeryBlackBrush �� 
TargetType ��Name �� CornerRadius	 J� SnapsToDevicePixels
 ��Name �� Triggers P� Property P� Value �� 
TargetName  
ResizeText  FontAwesome �� 	Resources  Key �� TargetProperty W� From	 W� To g� Duration g� AutoReverse g� RepeatBehavior �� Triggers �� Binding �� Self �� RelativeSource �� Path �� Value N� EnterActions ��Name 
	 Resize N� ExitActions �� BeginStoryboardName 
 TextBoxPlaceHolder  ��Name
! � Tag" ]� 
Conditions# �� Property$ �� Value�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      "localclr-namespace:Mycord_WPF  5        5      �� 5   
   $ FontsDictionary.xaml?�6   �� 5   
   $ ColorsDictionary.xaml?�6   %
  &	        7  7  7  7  7  &	 U    7  7 	 &	
 M    7  7  7  �� 5	      " ��6   8   .�5
   
   ��  � ��$	 0.5q�6,   �� 5   
    � ��8  6(   �� 5   
    � ��8  6'   �� 5   
    � ��8  6'   �� 5   
    � ��$ 10q�6$   �� 5   
    � ��$ 0 5 0 5q�6#   �� 5   
    � ��$ 17(�6%   �� 5   
    � ��8  6'   �� 5   
    � �� 5      �� 5      " ��6"   .R�5      � .+�5      �� $ border��6!   #��z��61   #��z��6]   #��z��6�   $ 4��5      $
	 True��6.   ._�5      �� $
 PART_ContentHost��6+   $|�False��6E   $��Hidden=�6W   $��Hidden=�6~   5      5       5      P� 5       � {�$ False��67   . �5      �� $ border��68    �@J�Opacity$
 0.56Q�6L   5      P� 5       �@J�IsMouseOver$
 True��69   . �5       �� $ border��6<    � �� �#3691CD6P   �� 5!      $ border��6@    � ��$ 1q�6T   5"      P� 5#       �@J�	IsFocused$
 True��67   . �5$      �� $ border��6<    � �� �#3691CD6P   �� 5%      $ border��6@    � ��$ 1q�6T   5&      5(      5*      5+      �� 5-      " ��6   .�5.   
   ��  � ��8   6'   �� 5/   
    � ��$	 ��6!    50   
   ���� 51      
Resize .��52      W� $ FontSize�53       1054       406   $ 0:0:0.5D�6'   $
 True��55      $ Forever��56      57      58       5:   
   �� 5;       6   �� # Z $ 	IsVisible� True6l    5<      �� 5=      $ ResizeStoryboard��6&   8�� 6>    5?      �� 5@      $ ResizeStoryboard��6'   5B      5D      �� 5F      " ��6   8   .�5G   
   ��  � ��$ 20(�6%   �� 5H   
    � ��$ Maplestory Light)�6'   �� 5I   
    � ��$ 0,0,0,1q�6,   �� 5J   
    � ��8  6(   �� 5K   
    � ��
 ���� 6'   �� 5L   
    � �� 5M      �� 5N      " ��6"   .R�5O      � .+�5P      �� #��z��6!   #��z��6K   #��z��6   $
	 True��6�   �� 5Q      $
 PART_ContentHost��6'   $��Center=�6A   �� 5R      $  WaterMarkLabel��6;   $
��Left=�6    #��z! 6S   $��Center=�6s   $y�	Collapsed=�6�   
�������8�� 6�   5S       5T      ]� 5U      " 5V      �� 5W   "   # �@J�	IsFocused$$ false��6A   �� 5X   "   # � ��$$  ��6<   ./�5Z      �� $ WaterMarkLabel��6;    � y�$ Visible=�6W   5[      P� 5\       � {�$ False��67   . �5]      ��  � ��
 �iii�6;   5^      5`      5b      5c      5e      