(define (domain ai_game)
	(:requirements :strips :typing :fluents :conditional-effects)
	(:types 	
		place - object		 
		person - object		
		resource - place			
	)
	
	(:functions
		(wood)
		(timber)
		(iron)
		(stone)
		(rifles)
		(stored-ore)
		(stored-coal)	
		(ore ?p - place)
		(coal ?p - place)
		(min_resource)		
		(time)
		(riflemen)
		(population)
	)
	
	(:predicates		
		(has-turfhut)
		(has-house)
		(has-sawmill)
		(has-smelter)
		(has-school)
		(has-storage)
		(has-oremine)
		(has-coalmine)
		(has-quarry)
		(has-forge)
		(has-barracks)		
		
		(ore_resource ?p - place)
		(coal_resource ?p - place)	

		(has-labourer)	
		(has-rifleman)
		(has-labourer)	
		(has-rifleman)
		(has-carpenter)	
		(has-lumberjack)
		(has-blacksmith)
		(has-teacher)
		(has-miner)				
	)
	
	;; ------------- ACTIONS ---------- ;;	
	
	;; ------------- Reproduction --------- ;;

	(:action reproduceTurfHut
			:parameters (?person - person)
			:precondition(has-turfhut)
			:effect(and (increase (population) 1))
	)
	
	(:action reproduceHouse
			:parameters(?person - person)
			:precondition(has-house)
			:effect(and (increase (population) 2))
	)
	
	;;------------- Training ------------- ;;	
	
	(:action trainCarpenter
			:parameters(?person - person)
			:precondition(and(not(has-carpenter)))
			:effect(and (has-carpenter)
					  (when(has-school) (increase(time) 50))
					  (when(not(has-school)) (increase(time) 100)))
	)
	
	(:action trainBlacksmith
			:parameters(?person - person)
			:precondition(and(not(has-blacksmith)))
			:effect(and (has-blacksmith)
					  (when(has-school) (increase(time) 50))
					  (when(not(has-school)) (increase(time) 100)))
	)
	
	(:action trainLumberjack
			:parameters(?person - person)
			:precondition(not(has-lumberjack))
			:effect(and (has-lumberjack)
					  (when(has-school) (increase(time) 50))
					  (when(not(has-school)) (increase(time) 100)))
	)
	
	(:action trainTeacher
			:parameters(?person - person)
			:precondition(and (not(has-teacher)))
			:effect(and (has-teacher)
					  (when(has-school) (increase(time) 50))
					  (when(not(has-school)) (increase(time) 100)))
	)

	(:action trainMiner
			:parameters(?person - person)
			:precondition(and (not(has-miner)))
			:effect(and (has-miner)
					  (when(has-school) (increase(time) 50))
					  (when(not(has-school)) (increase(time) 100)))
	)
	
	(:action trainRifleman
			:parameters(?person - person)
			:precondition(and (has-barracks) (>= (rifles) 1) (> (population) (riflemen)))
			:effect(and  (increase (riflemen) 1) (decrease (rifles) 1))
	)
	
	;; --------- Acquire Resource --------- ;;
	
	(:action cutTree
			:parameters(?person - person)
			:precondition(and(has-lumberjack))
			:effect(and (increase (timber) 1))
	)
	
	(:action simpleMineOre
			:parameters(?resource - resource)
			:precondition(and(> (ore ?resource) (min_resource)))
			:effect(and (increase (stored-ore) 1) (decrease (ore ?resource) 1))
	)
	
	(:action simpleMineCoal
			:parameters(?resource - resource)
			:precondition(and(> (coal ?resource) (min_resource)))
			:effect(and(increase(stored-coal) 1) (decrease (coal ?resource) 1))
	)	
	
	(:action mineOre
			:parameters(?person - person)
			:precondition(and(has-miner) (has-oremine))
			:effect(increase(stored-ore) 1)
	)
	
	(:action mineCoal
			:parameters(?person - person)
			:precondition(and(has-miner) (has-coalmine))
			:effect(increase(stored-coal) 1)
	)

	(:action cutStone
			:parameters(?person - person)
			:precondition(and (has-quarry))
			:effect(increase(stone) 1)
	)	
	
	(:action produceWood
			:parameters(?person - person)
			:precondition(and (has-sawmill) (>= (timber) 1))
			:effect(and(increase (wood) 1) (decrease (timber) 1))
	)
	
	(:action produceIron
			:parameters(?person - person)
			:precondition(and(>= (stored-coal) 1) (>= (stored-ore) 1) (has-smelter))
			:effect(and(increase(iron) 1) (decrease (stored-ore) 1) (decrease (stored-coal) 1))
	)
	
	(:action produceTool
			:parameters(?person - person)
			:precondition(and (has-blacksmith) (has-forge))
			:effect(increase(rifles) 1)
	)
	
	;; --------- Build Building ---------- ;;
	
	(:action buildTurfHut
			:parameters(?person - person)
			:precondition(and (not(has-turfhut)))
			:effect(and(has-turfhut))
	)
	
	(:action buildHouse
			:parameters(?person - person)
			:precondition(and (has-carpenter)  (not(has-house))
						  (>= (wood) 1))
			:effect(and(has-house) (decrease (wood) 1))
	)
	
	(:action buildSmelter
			:parameters(?person - person)
			:precondition(and (not(has-smelter)) 
						  (>= (stone) 1))
			:effect(and(has-smelter)  (decrease (stone) 1))
	)
	
	(:action buildSchool
			:parameters(?person - person)
			:precondition(and (not(has-school))
						  (>= (iron) 1)
						  (>= (wood) 1)
						  (>= (stone) 1))						  
			:effect(and(has-school) (decrease (iron) 1) (decrease (wood) 1) (decrease (stone) 1))
	)
	
	(:action buildSawMill
			:parameters(?person - person)
			:precondition(and (not(has-sawmill)) 
						  (>= (stone) 1) 
						  (>= (timber) 1) 
						  (>= (iron) 1))
			:effect(and(has-sawmill) (decrease (stone) 1) (decrease (iron) 1) (decrease (timber) 1))
	)
	
	(:action buildForge
			:parameters(?person - person)
			:precondition(and (not(has-forge))
						  (>= (stone) 1) 
						  (>= (timber) 1) 
						  (>= (iron) 1))
			:effect(and(has-forge) (decrease (stone) 1) (decrease (iron) 1) (decrease (timber) 1))
	)
	
	(:action buildOreMine
			:parameters(?resource - resource)
			:precondition(and(has-carpenter) (has-blacksmith) (ore_resource ?resource)  (not(has-oremine))
						  (>= (wood) 1)
						  (>= (iron) 1))
			:effect(and(has-oremine)  (decrease (wood) 1) (decrease (iron) 1))
	)
	
	(:action buildCoalMine
			:parameters(?resource - resource)
			:precondition(and(has-carpenter) (has-blacksmith) (coal_resource ?resource) (not(has-coalmine))
						  (>= (wood) 1)
						  (>= (iron) 1))
			:effect(and(has-coalmine) (decrease (wood) 1) (decrease (iron) 1))
	)		
	
	(:action buildBarracks
			:parameters(?person - person)
			:precondition(and(has-carpenter) (not(has-barracks))
						  (>= (stone) 1)
						  (>= (wood) 1))
			:effect(and (has-barracks) (decrease (stone) 1) (decrease (wood) 1))
	)
	
	(:action buildQuarry
			:parameters(?person - person)
			:precondition(and (not(has-quarry)))
			:effect(and (has-quarry))
	)	
)	
