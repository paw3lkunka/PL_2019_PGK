public enum KeyboardButtons
{
    unknown,

#region Special

    leftButton,
    middleButton,
    rightButton,

    trigger,

    escape,
    space,
    enter,
    numpadEnter,
    tab,
    capsLock,
    backspace,
    home,

    upArrow,
    downArrow,
    leftArrow,
    rightArrow,

    leftShift,
    shift = leftShift,
    rightShift,

    leftAlt,
    alt = leftAlt,
    rightAlt,

    leftCtrl,
    ctrl = leftCtrl,
    rightCtrl,

    contextMenu,

    scroll,

#endregion

#region Standard no change

    backquote, // `
    quote, // '
    semicolon, // ;
    comma, // ,
    period, // .
    slash, // /
    backslash, // \
    leftBracket, // [
    rightBracket, // ]
    minus, // -
    equals, // =

    a,
    b,
    c,
    d,
    e,
    f,
    g,
    h,
    i,
    j,
    k,
    l,
    m,
    n,
    o,
    p,
    q,
    r,
    s,
    t,
    u,
    v,
    w,
    x,
    y,
    z,
    
    k1,
    k2,
    k3,
    k4,
    k5,
    k6,
    k7,
    k8,
    k9,
    k0,

    end,
    insert,

    f1,
    f2,
    f3,
    f4,
    f5,
    f6,
    f7,
    f8,
    f9,
    f10,
    f11,
    f12,

#endregion

#region Standard change

    pageDown,
    pageUp,
    delete,
    pause,
    numLock,
    printScreen,
    scrollLock,

    numpadDivide,
    numpadMultiply,
    numpadPlus,
    numpadMinus,
    numpadPeriod,
    numpadEquals,
    numpad1,
    numpad2,
    numpad3,
    numpad4,
    numpad5,
    numpad6,
    numpad7,
    numpad8,
    numpad9,
    numpad0,

    #endregion

    #region Unrecognized

    leftMeta, // wtf is this?
    rightMeta = leftMeta,

    OEM1, // what the hell is this?
    OEM2,
    OEM3,
    OEM4,
    OEM5,

#endregion

    first_special = leftButton,
    last_special = scroll,

    first_standard_noChange = backquote,
    last_standard_noChange = f12,

    first_standard_change = pageDown,
    last_standard_change = numpad0,
}
