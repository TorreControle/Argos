select * from argos.t_painel_automation;
select * from argos.t_trabalho_divulgacao_automation;

select 
	a.ID as ID_PAINEL,
	a.COMANDO as COMANDO_PAINEL,
	a.NOME as NOME_REPORT,
	c.CHAT_ID CHAT_ID_GRUPO,
	c.NOME as NOME_GRUPO
from argos.t_painel_automation a
	join argos.t_painel_automation_grupo_telegram b on b.ID_PAINEL = a.ID 
	join argos.t_grupo_telegram c on c.CHAT_ID = b.CHAT_ID_GRUPO
where c.CHAT_ID = '-4050698543' and 
	a.TIPO_ENVIO = 'HIBRIDO';
	
select * from argos.t_trabalho_divulgacao_automation;