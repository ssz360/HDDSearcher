   M S B A M L   `   `   `  ����  HDD Searcher    HDD_Searcher.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  HDD Searcher��5   	   $	��350��6   $	��525��6+   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��1* �6   .+�5
   
   � $��10q�6    � ColumnDefinitions 5      �� 5      $��1* �6#   �� 5      $
��auto �6#   .+�5      �� -   5        $��	txtSearch��5      $��1q�6?   
�������$��Stretch=�5      $��Stretch=�62   $��5q�6P   �� 5      -   5        $��cbIcon��5   >    d� 	IsChecked$ FalseY�6   $��
Show icons��6)   $y�	Collapsed=�6L   �� 5      -   5        $��btnSettings��5      $��Settings��6B   $��1��6U   5      �� 5   
   -   5        $��lstView��5      $��Stretch=�6!   $��1��6F   ��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value 5      �� 5      .R�5   "   �� ���	#8B808080$��0 0 0 1q�6A   $��Stretch=�6[   $��Stretch=�5   )   ._�5   &   ��  �� Orientation$ 
Horizontal=�61   .+�5   *   �� $��imgIcon��5   H   $��0��5   0   $��0��6@   $��16��5   0   $	��5,0q�6;   6H   �� 5   *   $��1��6N   $��0��6^   ��60   ��  �� Path$
 Name�6^   5    '   5!   #   	 �� Triggers	 5"   "   �� 5#   &   
 �� Value$

 True��6i    �� Binding 62   ��  �� ElementName$ cbIcon��$ 	IsChecked�.a�5$   *   ��  �� 
TargetName$ imgIcon��61    6X   �� *Icon+ � ��5%   '   5'      5)      5*      5-      5.      5/      