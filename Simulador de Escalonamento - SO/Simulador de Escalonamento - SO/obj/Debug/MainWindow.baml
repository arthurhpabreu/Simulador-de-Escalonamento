   M S B A M L   `   `   `  ����Z  VSimulador de Escalonamento - SO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:Simulador_de_Escalonamento___SOSimulador_de_Escalonamento___SO  5   	   0    *Simulador_de_Escalonamento___SO.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ?local-clr-namespace:Simulador_de_Escalonamento___SO     5   	   -   5          � Title$  
MainWindow��5   	   $��553.21��6   $��1231.467��6,    � 
ResizeMode$ NoResize=�6=   .��5	      � .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ DataGridGerenciador��5
      $
��Left=�60   $��	447,8,0,0q�6K   $	��768��6^   $	��384��6j   $	��Top=�6w    � 	Resources 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property �� Value��%�   '        �� 5      "  6   .�5      ��  � ��$
 Bold"�63   �� 5       � ��
 �   �64   �� 5       � ��$ 2,2,0,2q�68   5      5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$	 PID��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$
	 auto 62   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6?   �� *PID+  5      $ Nome do processo��6%   $
	 auto 6?   
 6L   �� *NomeDoProcesso+  5      $ Estado do processo��6%   $
	 auto 6A   
 6N   �� *Estado+  5      $ 
Prioridade��6%   $
	 auto 69   
 6F   �� *
Prioridade+  5      $ Tempo em espera(ms)��6%   $
	 auto 6B   
 6O   �� *#!TempoEmEspera.ElapsedMilliseconds+  5      $ Ciclos restantes��6%   $	 * 6?   
 6I   �� *NumeroDeCiclos+5"      �� 5#   
   -   5         ��Name$ 
TxtQuantum��5#      $
��Left=�6&   $��23��6A   $��	78,10,0,0q�6M    �� TextWrapping$
 Wrap=�6`   $	��Top=�6t   $��58��6�    �� MaxLines$ 1��6�    �� MinLines$ 1��6�    �� IsUndoEnabled . 4  .System.Windows.Automation.AutomationProperties   IsRequiredForForm .   J� RenderTransformOrigin$ 
1.155,0.63&�6�   $��15000��6   �� 	UndoLimit$ 10000��6%  �� 5$   
   $��Quantum:��6   $
��Left=�6#   $��	10,10,0,0q�6>   $	��Top=�6Q   $ 0.567,-1.346&�6i    � ToolTip$ É o quantum ��6�   �� 5%   
   -   5         ��Name$ BtnIniciaSimulacao��5%      $��Iniciar simulação��6-   $
��Left=�6I   $��
10,245,0,0q�6d   $	��Top=�6x   $	��432��6�   $��30��6�   6�   �� 5&   
   $��Número de threads: ��6   $
��Left=�6.   $��
244,10,0,0q�6I   $	��Top=�6]   $ -1.711,0.808&�6u   �� 5'   
   $��Processo em execução:��6   $
��Left=�60   $��	3,396,0,0q�6K   $	��Top=�6^   �� 5(   
   -   5         ��Name$ LabelNomeProcesso��5(      $��Label��6+   $
��Left=�6;   $��135,396,0,0q�6V   $	��Top=�6k   $ 1.434,0.558&�6�   �� 5)   
   $
��PID:��6   $
��Left=�6   $��274,462,0,0q�6:   $	��Top=�6O   �� 5*   
   -   5        $ LabelPID��5*      $��Label��6"   $
��Left=�62   $��310,462,0,0q�6M   $	��Top=�6b   $ 0.5,0.5&�6z   �� 5+   
   $��Prioridade:��6   $
��Left=�6&   $��	3,428,0,0q�6A   $	��Top=�6T   $ 0.478,0.596&�6l   �� 5,   
   -   5        $ LabelPrioridade��5,      $��Label��6)   $
��Left=�69   $��
70,428,0,0q�6T   $	��Top=�6h   $ 0.066,0.558&�6�   �� 5-   
   $"��Tempo máximo de execução:��6   $
��Left=�64   $��	10,72,0,0q�6O   $	��Top=�6b   $y sIndica o tempo máximo de execução de um processo até que ele seja interrompido e tenha sua prioridade reduzida.��6z   �� 5.   
   $.��(Thread a exibir os dados de execução: ��6   $
��Left=�6A   $��
10,214,0,0q�6\   $	��Top=�6p   $B <Selecione o índice da thread que você deseja ver os dados.��6�   �� 5/   
   -   5        $ TxtTempoVerif��5/      $
��Left=�6)   $��25��6D   $��
224,73,0,0q�6P   $
 Wrap=�6d   $	��Top=�6x   $��58��6�   $ 1��6�   $��14540��6�   �� 50   
   $��Tempo em execução: ��6   $
��Left=�6.   $��274,428,0,0q�6I   $	��Top=�6^   �� 51   
   $*��$Tempo de atualização da interface:��6   $
��Left=�6=   $��
10,169,0,0q�6X   $	��Top=�6l   $ 0.5,0.5&�6�   $T NTempo recomendado: acima de 100ms. Valores inferiores podem gerar travamentos.��6�   �� 52   
   -	   5         ��Name$ ComboBoxThreadExibicao��52      $
��Left=�63   $��224,214,0,0q�6N   $	��Top=�6c   $��58��6{   $��26��6�   $ 0.047,1.769&�6�   {�.  6�   �� 53   
   -
   5        $ ComboBoxNumeroThreads��53      $
��Left=�62   $��
368,10,0,0q�6M   $	��Top=�6a   $��58��6y   $��23��6�    �� SelectedIndex$ 0��6�   .@�54      �� $��1��6   $
��Left=�6'   $��90��6B   �� 55      $��2��6   $
��Left=�6'   $��92��6B   �� 56      $��3��6   $
��Left=�6'   $��92��6B   �� 57      $��4��6   $
��Left=�6'   $��92��6B   58      �� 59   
   -   5        $ LabelTempoExec��59      $��Label��6(   $
��Left=�68   $��403,428,0,0q�6S   $	��Top=�6h   $ -0.395,-0.058&�6�   �� 5:   
   $��ms��6   $
��Left=�6   $��	141,8,0,0q�68   $	��Top=�6K   �� 5;   
   $��ms��6   $
��Left=�6   $��
287,72,0,0q�68   $	��Top=�6L   $ -1.62,0&�6d   �� 5<   
   -   5        $ TxtTempoInterface��5<      $
��Left=�6-   $��27��6H   $��224,169,0,0q�6T   $
 Wrap=�6i   $	��Top=�6}   $��58��6�   $	��100��6�   �� 5=   
   $��ms��6   $
��Left=�6   $	��Top=�68   $ 1.697,1.077&�6P   $��287,170,0,0q�6t   �� 5>   
   -   5        $ 	BtnPausar��5>      $��Pausar simulação��6$   $
��Left=�6?   $	��Top=�6Z   $	��432��6r   $��
10,280,0,0q�6~   $��31��6�   {�.  6�   �� 5?   
   -   5        $ 
BtnResumir��5?      $��Resumir simulação��6%   $
��Left=�6A   $��
10,316,0,0q�6\   $	��Top=�6p   $	��432��6�   $��30��6�   {�.  6�   �� 5@   
   $��Ciclos restantes:��6   $
��Left=�6,   $��	3,462,0,0q�6G   $	��Top=�6Z   �� 5A   
   -   5        $ LabelCiclco��5A      $��Label��6%   $
��Left=�65   $��
98,462,0,0q�6P   $	��Top=�6d   $ -6.908,2.058&�6|   �� 5B   
   $��Tempo máximo de espera:��6   $
��Left=�62   $��
10,120,0,0q�6M   $	��Top=�6a   $Q KIndica o tempo máximo que um processo espera até aumentar sua prioridade.��6y   �� 5C   
   -   5        $ TxtTempoEspera��5C      $
��Left=�6*   $��25��6E   $��224,120,0,0q�6Q   $
 Wrap=�6f   $��150000��6z   $	��Top=�6�   $��58��6�   �� 5D   
   $��ms��6   $
��Left=�6   $��287,120,0,0q�68   $	��Top=�6M   �� 5E   
   -   5        $ Btn_Interromper��5E      $��Parar simulação��6*   $
��Left=�6D   $��
11,351,0,0q�6_   $	��Top=�6s   $	��431��6�   $��30��6�   {�.  6�   5G      5H      