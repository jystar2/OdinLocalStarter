   M S B A M L   `   `   `  ����H  DLineCall, Version=10.0.19041.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:NCCaptureInjector32NCCaptureInjector32  5   	   (    "NCCaptureInjector32.AutoUpdatePage   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:NCCaptureInjector32     5   	   
�������  � 
ResizeMode$  CanMinimize=�5   7    � SizeToContent$ WidthAndHeight=�6P    � 	Resources 5      �� 5   
   %    5      .��5      � .+�5   
   �� $��1q�6    �� Orientation$ Vertical=�6    .+�5      �� -   5         ��Name$ 
UpdateText��5   .   $	��Top=�6   $	��270��6C   $	��Top=�6P   $��15(�6h   $��
10,10,10,0q�6v   $��Stretch=�6�   $�� ��6�   
���   �
�������$
��Auto=�6�   ]� 5      $��	10,0,10,0q�6   $��40��6-   $
��Auto��69   .+�5      �� $��Center=�6   $��16(�68   $��0,5,0,0q�6F   $��下載進度:��6X   
���   �
�������6y   � 5      -   5         �Name$ ProgressBarMax��5   ,   ���#B0B0B0$��12��6D   $��15,5,0,0q�6Q    J� IsHitTestVisible .  $��Stretch=�6|   .+�5      �� -   5         ��Name$ ProgressBar��5   >   $��12��6    $��0,0,0,0q�6,   $��0��6S    .  $
��Left=�6x   ��5      �� 5       �� Color$ #0030F0��6.   5!      �� 5#      -   5        $ label2��5#   ?   $	��0/0��6    
�������$��Center=�6P   $��Center=�6n    .  6�   5%      5&      ]� 5'      $��10,0,3,3q�6   $��30��6,   $
��Auto��68   .+�5(      �� $��Center=�6   $��16(�68   $��0,5,0,0q�6F   $��狀態:��6X   
���   �
�������6w   �� 5)      -   5        $ 
StatusText��5)   F   $��Center=�6   $��16(�68   $��0,5,0,0q�6Z   $�� ��6l   
���   �
�������6�   5+      5,      5-      5/      