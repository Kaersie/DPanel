   M S B A M L   `   `   `  ����A  =DPanel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:DPanelDPanel  5   	       DPanel.comTime   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:DPanel     5   	   _ [MaterialDesignThemes.Wpf, Version=5.1.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5
   	   -   5        $��75��5      $
��1050��6&     � WindowStyle$
  None=�63      MaterialDesignBody#���   5   	   $��HarmonyOS Sans SC)�5	      $��13(�5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	    � AllowsTransparency . ���	#FFE6D9D9��5	   	   Z� ��5      �� 5   
    �� 
TargetType" �6   .�5      ��  �� Property � �� �� Value 5      �� 5       �� 
TargetType" �6&   .R�5      �� 
������� �� CornerRadius$ 20��64   	 J� Opacity$		 0.8Q�6F   
 J� Effect
 5      3  -System.Windows.Media.Effects.DropShadowEffect  5   "      ShadowDepth$ 0Q�63      
BlurRadius$ 10Q�6C      Opacity$	 0.8Q�6S      Color$ #AAAAAA��6a   ._�5      �� 5      5      5      5      .��5       � -   5         �Name$
 grid��5       .+�5#   
   ]� .+�5$      �� $
��Left=�6    �� Orientation$ 
Horizontal=�60   .+�5%      �� -   5         ��Name$ 	TimeLabel��5%      $��00:00:00��6+   $
��Left=�6>   $��15,0,0,0q�6Y   $��Center=�6k   $
��15,5q�6�   $��30(�6�   $��Center=�6�   $��Center=�6�   �� 5&      -   5        $ DayLabel��5&      $��周六��6*   $
��Left=�67   $��0,0,0,0q�6S   $��Center=�6d   $��30(�6�   $��Center=�6�   $��Center=�6�   $
��15,0q�6�   5'      �� 5(      -   5        $ 	DateLabel��5(      $��Right=�6'   $��
2024/11/16��6>   $��Right=�6S   $��0,0,15,0q�6p   $��Center=�6�   $��30(�6�   $��Center=�6�   $��Center=�6�   $
��15,5q�6�   '  !MaterialDesignThemes.Wpf.PackIcon  5)      -   5          Name$ WeatherIcon��5)   &   $��Right=�6<      Kind$ WhiteBalanceSunny=�6S   $��0,0,10,0q�6l   $��Center=�6   $��Right=�6�   $��30(�6�   $��30��6�   $��30��6�   5*      �� 5+   
   $ 
Horizontal=�6   $��Center=�6.   .+�5,      �� -   5         ��Name$ IntelligentText��5,      $��30(�61   $��Center=�6?   扬帆起航，奔赴梦想6e   5-      5.      5/      