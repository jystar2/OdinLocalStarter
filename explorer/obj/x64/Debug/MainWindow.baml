   M S B A M L   `   `   `  ����H  DLineCall, Version=10.0.19041.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:NCCaptureInjector32NCCaptureInjector32  5   	   $    NCCaptureInjector32.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:NCCaptureInjector32     5   	     � Title$  特工啟動器��5   	   
������� � 
ResizeMode$ CanMinimize=�5	   7    � SizeToContent$ WidthAndHeight=�6P    � 	Resources 5          OptionMark.Static.Background �� Color  OptionMark.Static.Border  OptionMark.Static.Glyph # OptionMark.MouseOver.Background  OptionMark.MouseOver.Border  OptionMark.MouseOver.Glyph ! OptionMark.Pressed.Background  OptionMark.Pressed.Border  OptionMark.Pressed.Glyph "	 OptionMark.Disabled.Background 
 OptionMark.Disabled.Border  OptionMark.Disabled.Glyph  FocusVisual �� Property �� Value J� SnapsToDevicePixels �� StrokeDashArray  OptionMarkFocusVisual  hcs	 �� 
TargetType
 �� BasedOn �� 
TargetType �Name � ColumnDefinitions ��Name �� CornerRadius F�Name J� Opacity �� Stretch ��Name ��Name �� Triggers P� Property P� Value �� 
TargetName  CheckBoxStyle  Button.Static.Background  Button.Static.Border  Button.MouseOver.Background  Button.MouseOver.Border  Button.Pressed.Background  Button.Pressed.Border  Button.Disabled.Background  Button.Disabled.Border  Button.Disabled.Foreground � RowDefinitions �� ElementName �� Path J� Effect-  'System.Windows.Media.Effects.BlurEffect   Radius   
KernelType  ButtonStyle  
LabelStyle  TextBlockStyle  SettingPanelStyle �� 	Resources  ComboBoxStyle  TextBlockStyle2 
 TxtPwd
  }� Pen! >� 	Thickness" >� Brush# >� 
EndLineCap$ >� StartLineCap% >� DashCap& >� 	DashStyle' �� Dashes( �� Offset) }� Location+  %System.Windows.TextDecorationLocation   TxtShow�� 5   
   %�!  &	        &	 !     &	 B     &	 e     &	 �     &	 �     &	 �     &	 �     &	     &		 3    &	
 V    &	 y    &	 �    &	 F    &	 �    '�� F    7��7  7  7  7  7  7  7  7  7 	 7 
 7  7  7  7  7  7  7  &	 0    7��&	 R    &	 s    &	 �    &	 �    &	 �    &	 �    &	 "    &	 E    &	 h    '�� �    7��7  7  7  7  7  7  7  7  &	 �    7��&	 	    '�� �    7��&	 �    7��&	 �    7  7  '�� ^    7��'�� G    7��&	 �    7��&	     7��&	 �    &	  B    �� 5      $ #202020��6C   �� 5      $ #1A7FD2��6?   �� 5      $ 	#FFFFFFFF��6>   �� 5      $ 	#FFE6E6E6��6F   �� 5      $ #43A047��6B   �� 5      $ 	#FFE6E6E6��6A   �� 5      $ #43A047��6D   �� 5      $ 	#FF3C77DD��6@   �� 5      $ 	#FFFFFFFF��6?   �� 5      $ 	#FFE6E6E6��6E   �� 5      $ 	#FFBCBCBC��6A   �� 5      $ 	#FF707070��6@   �� 5      .�5      ��  � �� 5       �� 5!      .R�5"      �� $��2q�6(    . #��� ��6N   $	 1 2R�6�   $��1��6�   5#      5%      5&      �� 5'      .�5(      ��  � �� 5)      �� 5*      .R�5+      �� $��14,0,0,0q�6(    . #��� ��6U   $	 1 2R�6�   $��1��6�   5,      5.      5/      �� 53      "	 ��6    .�54      ��  � ��$
 Left=�6?   6N   55      �� 57      "	 ��6   8
   .�58      ��  �@�FocusVisualStyle8  66   �� 59       � ��
 �����61   �� 5:       � ��8  61   �� 5;       � ��
 �   �60   �� 5<       � ��$ 16(�6-   �� 5=       � ��$ Microsoft JhengHei UI)�6/   �� 5>       � ��$ 21��6+   �� 5?       � ��$ 2q�64   �� 5@       � ��$ 2q�64   �� 5A       � �� 5B      �� 5C      " ��6*   .R�5D      � $ templateRoot��6#   
������  .  5E   "   �� 5F   &   $
��Auto �67   �� 5G   &   $��* �67   .+�5I   "   �� $ checkBoxBorder��6)   $��1q�5J      $ 2��5K      #��z��5L      #��z��5M      #��z��5N      #��z��5O      #��z��5P      ._�5Q   &   � $ markGrid��6+   .+�5R   *   F� $ 
optionMark��6/   $��1q�6E   Y�����4� �� Ѽ�G dԊ Ѩ�D dԊ �    ^>O ��z r�5 ��@ ��\ 
ܺ��� @�j�  �>��4� �� 8�� $ 0Q�5T   M   $ UniformToFill=�6Z   �� 5U   *   $ indeterminateMark��64   $��1q�6P   
������ $ 0Q�6p   5V   '   5W   #   �� 5X   "   $ contentPresenter��63   $��1��5Y   "   #��z��5Z   "   #��z��5[   "   #��z��5\   "   |�.  ��. # z 5_   "   5`       5a      P� 5b   "    � �� . . �5d   &   �� 8   �@�FocusVisualStyle5e      �� 5f   &    � ��$ 4,0,0,0q�5g      5h   #   P� 5i   "    �@J�IsMouseOver . . �5k   &   �� $ checkBoxBorder��6-   8   � ��6_   �� 5l   &   $ checkBoxBorder��6-   8   � ��6a   �� 5m   &   $ 
optionMark��6-   8   � ��6V   �� 5n   &   $ indeterminateMark��6-   8   � ��6^   5o   #   P� 5p   "    � {� .  . �5r   &   �� $ checkBoxBorder��6-   8 	  � ��5t      �� 5u   &   $ checkBoxBorder��6-   8 
  � ��6`   �� 5v   &   $ 
optionMark��6-   8   � ��6U   �� 5w   &   $ indeterminateMark��6-   8   � ��6\   5x   #   P� 5y   "    � �� . . �5z   &   �� $ checkBoxBorder��6-   8   � ��6_   �� 5{   &   $ checkBoxBorder��6-   8   � ��6_   �� 5|   &   $ checkBoxBorder��6-   8   � ��6`   �� 5}   &   $ 
optionMark��6-    � ��
 ���� 6U   �� 5~   &   $ indeterminateMark��6-    � ��
 ���� 6\   5   #   P� 5�   "    �@d�	IsChecked$
 trueY�5�      . �5�   &   �� $ checkBoxBorder��6-   8   � ��6_   �� 5�   &   $ checkBoxBorder��6-   8   � ��6`   �� 5�   &   $ 
optionMark��6-    �@J�Opacity$ 1Q�6X   �� 5�   &   $ indeterminateMark��6-    �@J�Opacity$ 0Q�6_   5�   #   P� 5�   "    6A   Z�  �@d�	IsChecked. �5�   &   �� $ 
optionMark��6-    �@J�Opacity$ 0Q�6X   �� 5�   &   $ indeterminateMark��6-    �@J�Opacity$ 1Q�6_   5�   #   5�      5�      5�      �� 5�      "	 ��6*   8
   6o   �� 5�      $ #43A047��6?   �� 5�      $ 	#FF707070��6;   �� 5�      $ 	#FF6ADFA4��6B   �� 5�      $ 	#FF3CB161��6>   �� 5�      $ 	#FFC4F6D2��6@   �� 5�      $ 	#FF2C8B5C��6<   �� 5�      $ 	#FFF4F4F4��6A   �� 5�      $ 	#FFADB2B5��6=   �� 5�      $ 	#FF838383��6A   �� 5�      "	 ��6   8
   .�5�      ��  �@�FocusVisualStyle8  65   �� 5�       � ��
 �����6/   �� 5�       � �� �#1A7FD26/   �� 5�       � ��
 �����6/   �� 5�       � ��$ 16(�6-   �� 5�       � ��$ Microsoft JhengHei UI)�6/   �� 5�       � ��$ 60��6-   �� 5�       � ��$ 0q�64   �� 5�       � ��$ Center=�6?   �� 5�       � ��$ Center=�6=   �� 5�       � ��$	 5,2q�6,   �� 5�       � �� 5�      �� 5�      " ��6*   .R�5�      �� $ border��6%   $ 4��65   #��z��6F   $
��Auto��6p   ._�5�   "   �  5�   &   �� 5�   *   �� 5�   *   $	��11* �68   .+�5�   &   �� $��0,0,0,1q�6-   
�������$��2��6�    6[   �� $ button��$ CornerRadius� 5�   *     5�   .   $ 2Q�69   $ Gaussian=�6D   5�   '   �� 5�   &   $��0,1,0,1q�6-   
�������$��2q�6[   $	 0.7Q�6f   $��2��6�    6t   �� $ button��$ CornerRadius� 5�   *     5�   .   $ 2Q�69   $ Gaussian=�6D   5�   '   �� 5�   &   $ contentPresenter2��67   #��z��6R   #��z��6�   #��z��6�   $��Center=�6�   $��Center=�6  #��z��6"  $��1��6G  $��5,0,5,2q�6T  6g  5�   #   5�       5�      P� 5�   "    �@��IsDefaulted . . �5�   &   �� $ border��6D   # � ��6X    � ��5�   #   P� 5�   "    �@J�IsMouseOver . . �5�   &   �� $ border��6C   8   � ��6W   �� 5�   &   $ border��6D   8   � ��6X   5�   #   P� 5�   "    � �� . . �5�   &   �� $ border��6C   8   � ��6W   �� 5�   &   $ border��6D   8   � ��6X   5�   #   P� 5�   "    � {� .  . �5�   &   �� $ border��6C   8   � ��6W   �� 5�   &   $ border��6D   8   � ��6X   �� 5�   &   $ contentPresenter2��6O   8   � ��6n   5�   #   5�      5�      5�      �� 5�      "	 ��6(   8
   6i   �� 5�      "	 ��6)   .�5�      ��  � ��$ 16(�6-   �� 5�       � ��$ Microsoft JhengHei UI)�6/   �� 5�       � ��$ Center=�66   �� 5�       � ��
 �   �6/   5�      �� 5�      "	 ��6   8
   .�5�      ��  � ��$ 16(�6-   �� 5�       � ��$ Microsoft JhengHei UI)�6/   �� 5�       � ��$ Center=�66   �� 5�       � ��
 �   �6/   5�      �� 5�      "	 ��6+   8
   6r   �� 5�      "	 ��6.    5�      ���� 5�      '��       "	 ��6   8
   63   �� 5�      '��       "	 ��6   8
  62   5�      5�      �� 5�      "	 ��6   8
   .�5�      ��  � ��$ 16(�6-   �� 5�       � ��$
 Auto��6*   �� 5�       � ��$ 30��6+   �� 5�       � ��$ Center=�6=   �� 5�       � ��$ Consolas)�6/   5      �� 5     "	 ��6   8
   .�5     ��  � ��$ 16(�6-   �� 5      � ��$ Consolas)�6/   �� 5      � ��$ Center=�66   5     �� 5
     "	 ��6*   8
   6o   �� 5     "	 ��6,   8
   .�5     ��  � ��$ 16(�6-   �� 5      � ��$ Microsoft JhengHei UI)�6/   �� 5      � ��
 �   �6/   �� 5      � ��$ Center=�66   5     �� 5     "	 ��6#   .�5     ��  �@��TextDecorations 5     |� 5     }� 5       5  "   >� 5  &   $! 10Q�6*   
" �   �$# Round=�6I   $$ Round=�6\   $% Round=�6u   & 5  *   �� 5  .   $' 0.0,1.2R�68   $	( 0.6Q�6I   5   '   ) 5"  "    5#  &   Strikethrough=�6L   5%     5&     5*     �� 5+      � ��$ 30��6+   �� 5,      � ��$
 Auto��6*   �� 5-      � ��
 �����6/   �� 5.      � ��
 ���� 6/   �� 5/      � ��$ 16(�6-   6:   �� 50      � ��$ Microsoft JhengHei UI)�6/   6O   51     �� 53     "	 ��6%   .�55     ��  � ��$ 16(�6-   �� 56      � ��$
 Auto��6*   �� 57      � ��$ 30��6+   �� 58      � ��$ Center=�6=   �� 59      � ��
 �����6/   �� 5:      � ��
 �   �6/   �� 5;      � ��$ Microsoft JhengHei UI)�6/   5>     5@     .��5C     � .+�5D     �� -   5        $��1q�5D     * �� Orientation$* Vertical=�6$   .+�5G     �� #��[ 6   $��18(�69   $��35��6G   
�������$��Center=�6j   $
��Bold"�6�   $��基本資訊��6�   � 5H     $��10,3,3,3q�6   + � Orientation$+ 
Horizontal=�6/   $
��Auto��6H   $	��800��6W   $
��Left=�6f   .+�5I     �� #��[ 6    $��20(�6I   $��奧丁特工官網:��6W   $��5,1,5,0q�6f   $
��Bold"�6x   �� 5J     -   5        , ��Name$, 
linkLabel1��5J  F   #��[ 6   $��https://ai.igcps.com��6Z   - � Cursor$
- Hand��6w   $
��Bold"�6�   $��10,1,5,0q�6�   
����  �$��	Underline{�6�   6�   5K     � 5M     $��10,0,3,3q�6   $+ 
Horizontal=�6/   $
��Auto��6H   $	��800��6W   $
��Left=�6f   .+�5N     �� #��[ 6   $��20(�6E   $"��奧丁特工遊戲端版本:��6S   $��2,1,5,0q�6e   $
��Bold"�6w   �� 5O     -   5        $, VersionLabel��5O  W   #��[ 6    $��20(�6I   $
��2023��6m   $
��Bold"�6{   $��10,1,5,0q�6�   �� 5P     -   5        $, NewVersionLabel��5P  W   #��[ 6    $��20(�6I   $�� ��6p   $
��Auto��6x   $	��600��6�   $
��Wrap=�6�   $
- Hand��6�   $
��Bold"�6�   $��10,4,5,4q�6�   ���	#FF405DDC$��	Underline{�6�   6  �� 5R     -   5        . ��Name$#. 檢查版本ToolStripMenuItem��5R     $��檢查版本��6>   $
��Left=�6M   $��32��6h   $	��5,2q�6u   $��20,3,0,0q�6�   $��60��6�   $	��Top=�6�   $
��Auto��6�   $��0q�6�   5T     �� 5V     -   5        / ��Name$/ VerticalPanel2��5V  (   $��1q�6   $* Vertical=�6@   �� 5X     -   5        $/ VerticalPanel1��5X  (   $��1q�6   $* Vertical=�6@   .+�5Y     �� -   5        0 ��Name$0 Header0��5Y  �   #��[ 6   $��18(�6=   $��35��6K   
�������$��Center=�6n   $
��Bold"�6�   $��啟動��6�   �� 5[     -	   5        $0 Header1��5[  �   #��[ 6   $��18(�6<   $��35��6J   ���#414141$��Center=�6k   $
��Bold"�6�   $(��" (注意:只能選64位元進程!)��6�   $y�	Collapsed=�6�   � 5\     $��0,0,3,3q�6   $+ 
Horizontal=�6.   $
��Auto��6G   $	��800��6V   $
��Left=�6e   .+�5^     �� #��[ 6    $��20(�6I   $��模式:��6W   $��10,5,5,0q�6b   $
��Bold"�6u   �� 5`     -
   5        1 ��Name$1 ModeList��5`      
2 � Tag$2 OpenMode��62   3 �� 
IsEditable3 .  $
��Bold"�6T   $��18(�6g   $��30��6u   4 �� Text$4  ��6�   $��10,8,0,0q�6�   $	��200��6�   $	��350��6�   $��Center=�6�   6  5c     � 5e     -   5        5 �Name$5 SetLinkPanel��5e  �   $��0,3,3,3q�6   $+ 
Horizontal=�6.   $
��Auto��6G   $	��800��6V   $
��Left=�6e   $y�	Collapsed=�6�   .+�5f     �� $��1q�6"   $* Vertical=�6-   .+�5h     �� #��[ 6$   $��20(�6M   $0��*(不勾選則默認使用Notepad記事本)��6[   $��10,5,5,0q�6w   $
��Bold"�6�   � 5j     $��0,3,3,3q�6%   $+ 
Horizontal=�66   $
��Auto��6O   $	��800��6^   $
��Left=�6m   .+�5l     �� -   5        6 ��Name$6 使用其他進程��5l  (   $��使用其他進程:��68   $��20(�6J   $��Bottom=�6X   $
��Left=�6z   $��25��6�   $��	13,10,0,0q�6�   $	��Top=�6�   $
��Auto��6�   7 d� 	IsChecked$7 FalseY�6�   �� 5m     -   5        8 ��Name$8 其他進程路徑��5m  '   $2  ��67   $
��Bold"�6>   $
��Left=�6P   $	��260��6k   $��Microsoft JhengHei UI)�6w   9 �� TextWrapping$
9 Wrap=�6�   $
��Left=�6�   $
��Auto��6�   $��35��6�   $��10,5,0,1q�6�   $��Center=�6  �� 5o     -   5        $. 其他進程路徑瀏覽��5o  &   $��瀏覽��69   $
��Left=�6F   $��32��6a   $��10,6,0,0q�6n   $	��Top=�6�   $
��Auto��6�   $��0q�6�   �� 5q     -   5        $6 不新建進程��5q  (   $��不新建進程��67   $��20(�6G   $y�	Collapsed=�6U   $��Bottom=�6l   $
��Left=�6�   $��25��6�   $��	20,10,0,0q�6�   $	��Top=�6�   $
��Auto��6�   $7 FalseY�6�   5r     5s     5t     � 5v     $��0,3,3,3q�6   $+ 
Horizontal=�6.   $
��Auto��6G   $	��800��6V   $
��Left=�6e   $y�	Collapsed=�6�   .+�5w     �� #��[ 6(   $��目標啟動程序:��6Q   $��20(�6`   $��10,1,5,0q�6n   $
��Bold"�6�   �� 5x     -   5        $1 ProcessList��5x  $   3 .  $��18(�6M   $��25��6[   $4  ��6g   $��10,5,0,0q�6o   $	��150��6�   $	��350��6�   $��Center=�6�   6�   5z     � 5}     $��0,5,3,3q�6!   $+ 
Horizontal=�62   $
��Auto��6K   $	��800��6Z   $
��Left=�6i   .+�5     �� #��[ 6$   $y�	Collapsed=�6M   $��狀態:��6d   $��20(�6o   $��10,1,5,0q�6}   $
��Bold"�6�   �� 5�     -   5        $, 
StatusText��5�  d   #��[ 6$   $y�	Collapsed=�6M   $��	未設定��6x   $��20(�6�   $��10,1,5,0q�6�   $
��Bold"�6�   �� 5�     -   5        $. StartSetButton��5�     $��啟動特工��66   $��28(�6E   $
��Bold"�6S   $
��Left=�6e   $��50��6�   $	��5,2q�6�   $��10,4,0,0q�6�   $��70��6�   $	��Top=�6�   $	��500��6�   $��0q�6   5�     5�     �� 5�     -   5        $0 Label1��5�  U   $	��Top=�6   $y�	Collapsed=�61   $��0��6H   #��[ 6e   $��18(�6�   $��35��6�   ���#414141$��Center=�6�   $
��Bold"�6�   $��系統資訊��6�   �� 5�     -   5        $8 	SystemLog��5�  K   $	��Top=�6   $y�	Collapsed=�64   $	��200��6_   $��Stretch=�6l   $��16(�6�   $��3,5,0,0q�6�   $��Stretch=�6�   $�� ��6�   : �� 
IsReadOnly: . 
�������$
��Left=�6�   $	��Top=�6  ���#303030$
��Auto=�6L  6|  5�     5�     5�     5�     