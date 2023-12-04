: first-last-digit
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
: parse-line first-last-digit 2rot 2 pick + 2rot swap 4 pick - swap 2rot drop 48 - swap 48 - 10 * + ;
variable sum
: advcodeloop1_1  
  parse-line sum @ + sum ! dup 0> 
  IF     recurse
  ELSE   
  THEN ;
: advcode1_1  
  0 sum ! advcodeloop1_1 ;
create buffer 1000000 allot
: open-read R/O open-file drop dup dup file-size drop drop buffer swap rot read-file drop swap close-file drop ;
0 S" input1_1" open-read buffer swap advcode1_1 sum @ CR . CR bye