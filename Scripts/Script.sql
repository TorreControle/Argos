-- Adicionar um trabalho novo se precisar.
select * from argos.t_trabalho_divulgacao_automation;

-- Adicionar o BI na tabela de paineis.
select * from argos.t_painel_automation;

-- Vincular a um grupo
select * from argos.t_painel_automation_grupo_telegram;
select * from argos.t_grupo_telegram;

-- Vincular a um horario de divulgação.
select * from argos.t_painel_automation_hora_divulgacao;
select * from argos.t_horario_divulgacao_automation;

-- Vincular a uma query de governança.
select * from argos.t_painel_automation_query_governanca;
select * from argos.t_query_governanca;

select 
	x.ID,
	x.NOME,
	x.GERANDO,
	x.HORA
from (select 
		distinct(a.ID) as ID,
		a.NOME as NOME,	
		a.GERANDO as GERANDO,
		IFNULL(e.HORA, 'Sem hora especifica') as HORA
	from argos.t_painel_automation a
		join argos.t_painel_automation_grupo_telegram b on b.ID_PAINEL = a.ID 
		join argos.t_painel_automation_hora_divulgacao d on d.PAINEL_ID = a.ID 
		join argos.t_horario_divulgacao_automation e on e.ID = d.HORA_DIVULGACAO_ID
		join argos.t_trabalho_divulgacao_automation f on f.ID = e.ID) x
where x.HORA = 'Sem hora especifica';

select 
	x.ID,
	x.COMANDO,
	x.NOME_PAINEL,
	x.LINK,
	x.CHAT_ID_GRUPO,
	x.NOME_GRUPO,
	x.VERTICAL_NEGOCIO,
	x.HORA_REPORT,
	x.NOME_ROTINA,
	x.EXPRESSAO_CRON,
	x.TIPO_ENVIO,
	x.ATIVO,
	x.GERANDO
from ( select 
			a.ID as ID,
			a.COMANDO as COMANDO,
			a.NOME as NOME_PAINEL,
			a.LINK as LINK,
			c.CHAT_ID as CHAT_ID_GRUPO,
			c.NOME as NOME_GRUPO,
			c.OPERACAO as VERTICAL_NEGOCIO,
			IFNULL(e.HORA, 'Sem hora especifica') as HORA_REPORT,
			f.NOME as NOME_ROTINA,
			f.EXPRESSAO_CRON as EXPRESSAO_CRON,
			a.TIPO_ENVIO as TIPO_ENVIO,
			a.ATIVO as ATIVO,
			a.GERANDO as GERANDO
		from argos.t_painel_automation a
			join argos.t_painel_automation_grupo_telegram b on b.ID_PAINEL = a.ID 
			join argos.t_grupo_telegram c on c.CHAT_ID = b.CHAT_ID_GRUPO 
			join argos.t_painel_automation_hora_divulgacao d on d.PAINEL_ID = a.ID  
			join argos.t_horario_divulgacao_automation e on e.ID  = d.HORA_DIVULGACAO_ID 
			join argos.t_trabalho_divulgacao_automation f on f.ID = e.ID_TRABALHO) x
where x.ID = '55' 
and x.HORA_REPORT = 'Sem hora especifica';

select * from argos.t_query_governanca;
select * from argos.t_query_governanca_grupo_telegram;
select * from argos.t_grupo_telegram;

select 
	a.GRUPO_DADOS,
	c.NOME 
from argos.t_query_governanca a
	join argos.t_query_governanca_grupo_telegram b on b.ID_QUERY = a.ID
	join argos.t_grupo_telegram c on c.CHAT_ID = b.CHAT_ID_GRUPO;


