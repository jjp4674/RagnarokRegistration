function valType(oSrc, args) {
    if ($('[name$="RegType"]').is(':checked'))
    {
        args.IsValid = true;
    }
    else
    {
        args.IsValid = false;
    }
}