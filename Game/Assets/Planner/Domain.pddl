(define (domain ai_game)
	(:requirements :strips :typing :fluents :conditional-effects)
	(:types 	
		place - object		 
		person - object
		building - place
		forest - place
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
		(has-blacksmithy)
		(has-barracks)		
		
		(ore_resource ?p - place)
		(coal_resource ?p - place)	

		(has-carpenter)	
		(has-lumberjack)
		(has-blacksmith)
		(has-teacher)
		(has-miner)				
	)
	
	;; ------------- ACTIONS ---------- ;;	
	
	;; ------------- Reproduction --------- ;;

	(:action reproduceTurfHut
			:parameters ()
			:precondition(has-turfhut)
			:effect(and (increase (population) 1))
	)
	
	(:action reproduceHouse
			:parameters()
			:precondition(has-house)
			:effect(and (increase (population) 2))
	)
	
	;;------------- Training ------------- ;;	
	
	(:action trainCarpenter
			:parameters()
			:precondition(and(has-teacher) (not(has-carpenter)))
			:effect(and (has-carpenter)
					  (when(has-school) (increase(time) 50))
					  (when(not(has-school)) (increase(time) 100)))
	)
	
	(:action trainBlacksmith
			:parameters()
			:precondition(and(has-teacher) (not(has-blacksmith)))
			:effect(and (has-blacksmith)
					  (when(has-school) (increase(time) 50))
					  (when(not(has-school)) (increase(time) 100)))
	)
	
	(:action trainLumberjack
			:parameters()
			:precondition(and(has-teacher) (not(has-lumberjack)))
			:effect(and (has-lumberjack)
					  (when(has-school) (increase(time) 50))
					  (when(not(has-school)) (increase(time) 100)))
	)
	
	(:action trainTeacher
			:parameters()
			:precondition(and (not(has-teacher)))
			:effect(and (has-teacher)
					  (when(has-school) (increase(time) 50))
					  (when(not(has-school)) (increase(time) 100)))
	)

	(:action trainMiner
			:parameters()
			:precondition(and(has-teacher) (not(has-miner)))
			:effect(and (has-miner)
					  (when(has-school) (increase(time) 50))
					  (when(not(has-school)) (increase(time) 100)))
	)
	
	(:action trainRifleman
			:parameters()
			:precondition(and (>= (rifles) 1) (> (population) (riflemen)))
			:effect(and  (increase (riflemen) 1) (decrease (rifles) 1))
	)
	
	;; --------- Acquire Resource --------- ;;
	
	(:action cutTree
			:parameters()
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
			:parameters()
			:precondition(and(has-miner) (has-oremine))
			:effect(increase(stored-ore) 1)
	)
	
	(:action mineCoal
			:parameters(?miner - person)
			:precondition(and(has-miner) (has-coalmine))
			:effect(increase(stored-coal) 1)
	)

	(:action cutStone
			:parameters()
			:precondition(and (has-quarry))
			:effect(increase(stone) 1)
	)	
	
	(:action produceWood
			:parameters()
			:precondition(and (has-sawmill) (>= (timber) 1))
			:effect(and(increase (wood) 1) (decrease (timber) 1))
	)
	
	(:action produceIron
			:parameters()
			:precondition(and(>= (stored-coal) 1) (>= (stored-ore) 1) (has-smelter))
			:effect(and(increase(iron) 1) (decrease (stored-ore) 1) (decrease (stored-coal) 1))
	)
	
	(:action produceTool
			:parameters(?blacksmith - person)
			:precondition(and (has-blacksmith) (has-blacksmithy))
			:effect(increase(rifles) 1)
	)
	
	;; --------- Build Building ---------- ;;
	
	(:action buildTurfHut
			:parameters()
			:precondition(and (not(has-turfhut)))
			:effect(and(has-turfhut))
	)
	
	(:action buildHouse
			:parameters()
			:precondition(and (has-carpenter)  (not(has-house))
						  (>= (wood) 1))
			:effect(and(has-house) (decrease (wood) 1))
	)
	
	(:action buildSmelter
			:parameters()
			:precondition(and (not(has-smelter)) 
						  (>= (stone) 1))
			:effect(and(has-smelter)  (decrease (stone) 1))
	)
	
	(:action buildSchool
			:parameters()
			:precondition(and (not(has-school))
						  (>= (iron) 1)
						  (>= (wood) 1)
						  (>= (stone) 1))						  
			:effect(and(has-school) (decrease (iron) 1) (decrease (wood) 1) (decrease (stone) 1))
	)
	
	(:action buildSawMill
			:parameters()
			:precondition(and (not(has-sawmill)) 
						  (>= (stone) 1) 
						  (>= (timber) 1) 
						  (>= (iron) 1))
			:effect(and(has-sawmill) (decrease (stone) 1) (decrease (iron) 1) (decrease (timber) 1))
	)
	
	(:action buildBlacksmith
			:parameters()
			:precondition(and (not(has-blacksmithy))
						  (>= (stone) 1) 
						  (>= (timber) 1) 
						  (>= (iron) 1))
			:effect(and(has-blacksmithy) (decrease (stone) 1) (decrease (iron) 1) (decrease (timber) 1))
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
			:parameters()
			:precondition(and(has-carpenter) (not(has-barracks))
						  (>= (stone) 1)
						  (>= (wood) 1))
			:effect(and (has-barracks) (decrease (stone) 1) (decrease (wood) 1))
	)
	
	(:action buildQuarry
			:parameters()
			:precondition(and (not(has-quarry)))
			:effect(and (has-quarry))
	)	
)	
