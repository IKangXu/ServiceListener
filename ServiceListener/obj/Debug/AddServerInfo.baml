   M S B A M L   `   `   `  ����J  FServiceListener, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ServiceListenerServiceListener  5   	   #    ServiceListener.AddServerInfo   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:ServiceListener     5   	   T PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3f;Controls,http://metro.mahapps.com/winfx/xaml/controls  5   	     � Title$  新增服务信息��5	   	   $��221.622��6   $��324.324��6)    J� 	AllowDrop .  � Topmost .  � WindowState$ Normal=�6Y    � WindowStartupLocation$ CenterScreen=�6n    � 
ResizeMode$ CanMinimize=�6�   .��5
      �� $��0,0,0,0q�6   .+�5   
   �  �� Left$ 0��6   
 �� Top$ 0��6   $	��181��6.   $	��306��6;   
�������.+�5      �� $��本机IP��6   $
��Left=�6#   $��	18,41,0,0q�6>   $	��Top=�6Q    J� RenderTransformOrigin$ -0.007,-0.392&�6i   �� 5      $��端口��6   $
��Left=�6!   $��	18,71,0,0q�6<   $	��Top=�6O   $ -0.007,-0.392&�6g   �� 5      $��路径��6   $
��Left=�6!   $��
18,101,0,0q�6<   $	��Top=�6P   $ -0.007,-0.392&�6h   �� 5      -   5        $��Id��5   [   $y�Hidden=�6   +  %MahApps.Metro.Controls.ControlsHelper	   FocusBorderBrush
	 �   �$
��Left=�6e   $��23��6�   $��	80,41,0,0q�6�   
 �� TextWrapping$

 Wrap=�6�   $	��Top=�6�   $	��220��6�   
�������
���   � �� 
CaretBrush
 �   �6�   �� 5      -   5        $��LocalIP��5   G   
	 �   �*  $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ 	127.0.0.1��6V   $��	127.0.0.1��6�   $
��Left=�6�   $��23��6�   $��	80,41,0,0q�6�   $

 Wrap=�6�   $	��Top=�6�   $	��220��6�   
�������
���   �
 �   �6,  �� 5      -   5        $
��Port��5   G   
	 �   �$ 请填写端口��6S   $
��Left=�6|   $��23��6�   $��	80,71,0,0q�6�   $

 Wrap=�6�   $	��Top=�6�   $	��220��6�   
�������
���   �
 �   �6  �� 5      -   5        $
��Path��5   G   
	 �   �$ 请填写路径��6S   $
��Left=�6|   $��23��6�   $��
80,101,0,0q�6�   $

 Wrap=�6�   $	��Top=�6�   $	��220��6�   
�������
���   �
 �   �6  �� 5      -   5        $��SureBtn��5      $
��Left=�6$   $��225,147,0,0q�6?   $	��Top=�6T   $��75��6l   $ -0.598,0.456&�6w   
�������.��5      �� $�确认��6   
���   �6    5      5      5      5      