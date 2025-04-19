#12xx or 13xx

for i in range(1234,1399):
    temp = str(i)
    check = False
    for j in range(len(temp)):
        if(check):
            break
        for k in range(j, len(temp)):
            if temp[j] == temp[k]:
                check = True
                break
    if(check):
        break
    check = False
    tempAns = str(i * 728)
    if(i * 728 > 1000000):
        break
    for j in range(len(tempAns)):
        if(check):
            break
        for k in range(j, len(tempAns)):
            if tempAns[j] == tempAns[k]:
                check = True
                break
    if(check):
        break
    print(tempAns)