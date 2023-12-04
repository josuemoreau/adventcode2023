: test  
  0 0 0 3 pick 0 
  DO     drop 3 pick i + c@ dup 10 = 
         IF     drop i 1+ LEAVE 
         ELSE   
         THEN 
         dup 48 >= over 57 <= and 
         IF     2 pick 0= 
                IF     rot drop dup rot rot 
                ELSE   
                THEN 
                nip 
         ELSE   drop 
         THEN 
         i' 
  LOOP
  ;
: test1 test 2rot 2 pick + 2rot swap 4 pick - swap 2rot drop 48 - swap 48 - 10 * + ;
: test2  
  dup 0 
  DO     i 1+ pick i + c@ 
  LOOP
  ;
\ : free  
\   depth 0> 
\   IF     depth 0 
\          DO     drop 
\          LOOP
         
\   ELSE   
\   THEN ;
variable sum
: advcodeloop1_1  
  test1 sum @ + sum ! dup 0> 
  IF     recurse
  ELSE   
  THEN ;
: advcode1_1  
  0 sum ! advcodeloop1_1 ;
0 s\" 1abc2\npqr3stu8vwx\na1b2c3d4e5f\ntreb7uchet" advcode1_1 sum @ CR . CR
bye