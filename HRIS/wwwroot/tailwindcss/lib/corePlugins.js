"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.default = _default;

var _preflight = _interopRequireDefault(require("./plugins/preflight"));

var _container = _interopRequireDefault(require("./plugins/container"));

var _accessibility = _interopRequireDefault(require("./plugins/accessibility"));

var _appearance = _interopRequireDefault(require("./plugins/appearance"));

var _backgroundAttachment = _interopRequireDefault(require("./plugins/backgroundAttachment"));

var _backgroundColor = _interopRequireDefault(require("./plugins/backgroundColor"));

var _backgroundPosition = _interopRequireDefault(require("./plugins/backgroundPosition"));

var _backgroundRepeat = _interopRequireDefault(require("./plugins/backgroundRepeat"));

var _backgroundSize = _interopRequireDefault(require("./plugins/backgroundSize"));

var _borderCollapse = _interopRequireDefault(require("./plugins/borderCollapse"));

var _borderColor = _interopRequireDefault(require("./plugins/borderColor"));

var _borderRadius = _interopRequireDefault(require("./plugins/borderRadius"));

var _borderStyle = _interopRequireDefault(require("./plugins/borderStyle"));

var _borderWidth = _interopRequireDefault(require("./plugins/borderWidth"));

var _boxSizing = _interopRequireDefault(require("./plugins/boxSizing"));

var _cursor = _interopRequireDefault(require("./plugins/cursor"));

var _display = _interopRequireDefault(require("./plugins/display"));

var _flexDirection = _interopRequireDefault(require("./plugins/flexDirection"));

var _flexWrap = _interopRequireDefault(require("./plugins/flexWrap"));

var _alignItems = _interopRequireDefault(require("./plugins/alignItems"));

var _alignSelf = _interopRequireDefault(require("./plugins/alignSelf"));

var _justifyContent = _interopRequireDefault(require("./plugins/justifyContent"));

var _alignContent = _interopRequireDefault(require("./plugins/alignContent"));

var _flex = _interopRequireDefault(require("./plugins/flex"));

var _flexGrow = _interopRequireDefault(require("./plugins/flexGrow"));

var _flexShrink = _interopRequireDefault(require("./plugins/flexShrink"));

var _order = _interopRequireDefault(require("./plugins/order"));

var _float = _interopRequireDefault(require("./plugins/float"));

var _clear = _interopRequireDefault(require("./plugins/clear"));

var _fontFamily = _interopRequireDefault(require("./plugins/fontFamily"));

var _fontWeight = _interopRequireDefault(require("./plugins/fontWeight"));

var _height = _interopRequireDefault(require("./plugins/height"));

var _lineHeight = _interopRequireDefault(require("./plugins/lineHeight"));

var _listStylePosition = _interopRequireDefault(require("./plugins/listStylePosition"));

var _listStyleType = _interopRequireDefault(require("./plugins/listStyleType"));

var _margin = _interopRequireDefault(require("./plugins/margin"));

var _maxHeight = _interopRequireDefault(require("./plugins/maxHeight"));

var _maxWidth = _interopRequireDefault(require("./plugins/maxWidth"));

var _minHeight = _interopRequireDefault(require("./plugins/minHeight"));

var _minWidth = _interopRequireDefault(require("./plugins/minWidth"));

var _objectFit = _interopRequireDefault(require("./plugins/objectFit"));

var _objectPosition = _interopRequireDefault(require("./plugins/objectPosition"));

var _opacity = _interopRequireDefault(require("./plugins/opacity"));

var _outline = _interopRequireDefault(require("./plugins/outline"));

var _overflow = _interopRequireDefault(require("./plugins/overflow"));

var _padding = _interopRequireDefault(require("./plugins/padding"));

var _placeholderColor = _interopRequireDefault(require("./plugins/placeholderColor"));

var _pointerEvents = _interopRequireDefault(require("./plugins/pointerEvents"));

var _position = _interopRequireDefault(require("./plugins/position"));

var _inset = _interopRequireDefault(require("./plugins/inset"));

var _resize = _interopRequireDefault(require("./plugins/resize"));

var _boxShadow = _interopRequireDefault(require("./plugins/boxShadow"));

var _fill = _interopRequireDefault(require("./plugins/fill"));

var _stroke = _interopRequireDefault(require("./plugins/stroke"));

var _strokeWidth = _interopRequireDefault(require("./plugins/strokeWidth"));

var _tableLayout = _interopRequireDefault(require("./plugins/tableLayout"));

var _textAlign = _interopRequireDefault(require("./plugins/textAlign"));

var _textColor = _interopRequireDefault(require("./plugins/textColor"));

var _fontSize = _interopRequireDefault(require("./plugins/fontSize"));

var _fontStyle = _interopRequireDefault(require("./plugins/fontStyle"));

var _textTransform = _interopRequireDefault(require("./plugins/textTransform"));

var _textDecoration = _interopRequireDefault(require("./plugins/textDecoration"));

var _fontSmoothing = _interopRequireDefault(require("./plugins/fontSmoothing"));

var _letterSpacing = _interopRequireDefault(require("./plugins/letterSpacing"));

var _userSelect = _interopRequireDefault(require("./plugins/userSelect"));

var _verticalAlign = _interopRequireDefault(require("./plugins/verticalAlign"));

var _visibility = _interopRequireDefault(require("./plugins/visibility"));

var _whitespace = _interopRequireDefault(require("./plugins/whitespace"));

var _wordBreak = _interopRequireDefault(require("./plugins/wordBreak"));

var _width = _interopRequireDefault(require("./plugins/width"));

var _zIndex = _interopRequireDefault(require("./plugins/zIndex"));

var _gap = _interopRequireDefault(require("./plugins/gap"));

var _gridAutoFlow = _interopRequireDefault(require("./plugins/gridAutoFlow"));

var _gridTemplateColumns = _interopRequireDefault(require("./plugins/gridTemplateColumns"));

var _gridColumn = _interopRequireDefault(require("./plugins/gridColumn"));

var _gridColumnStart = _interopRequireDefault(require("./plugins/gridColumnStart"));

var _gridColumnEnd = _interopRequireDefault(require("./plugins/gridColumnEnd"));

var _gridTemplateRows = _interopRequireDefault(require("./plugins/gridTemplateRows"));

var _gridRow = _interopRequireDefault(require("./plugins/gridRow"));

var _gridRowStart = _interopRequireDefault(require("./plugins/gridRowStart"));

var _gridRowEnd = _interopRequireDefault(require("./plugins/gridRowEnd"));

var _transform = _interopRequireDefault(require("./plugins/transform"));

var _transformOrigin = _interopRequireDefault(require("./plugins/transformOrigin"));

var _scale = _interopRequireDefault(require("./plugins/scale"));

var _rotate = _interopRequireDefault(require("./plugins/rotate"));

var _translate = _interopRequireDefault(require("./plugins/translate"));

var _skew = _interopRequireDefault(require("./plugins/skew"));

var _transitionProperty = _interopRequireDefault(require("./plugins/transitionProperty"));

var _transitionTimingFunction = _interopRequireDefault(require("./plugins/transitionTimingFunction"));

var _transitionDuration = _interopRequireDefault(require("./plugins/transitionDuration"));

var _configurePlugins = _interopRequireDefault(require("./util/configurePlugins"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _default({
  corePlugins: corePluginConfig
}) {
  return (0, _configurePlugins.default)(corePluginConfig, {
    preflight: _preflight.default,
    container: _container.default,
    accessibility: _accessibility.default,
    appearance: _appearance.default,
    backgroundAttachment: _backgroundAttachment.default,
    backgroundColor: _backgroundColor.default,
    backgroundPosition: _backgroundPosition.default,
    backgroundRepeat: _backgroundRepeat.default,
    backgroundSize: _backgroundSize.default,
    borderCollapse: _borderCollapse.default,
    borderColor: _borderColor.default,
    borderRadius: _borderRadius.default,
    borderStyle: _borderStyle.default,
    borderWidth: _borderWidth.default,
    boxSizing: _boxSizing.default,
    cursor: _cursor.default,
    display: _display.default,
    flexDirection: _flexDirection.default,
    flexWrap: _flexWrap.default,
    alignItems: _alignItems.default,
    alignSelf: _alignSelf.default,
    justifyContent: _justifyContent.default,
    alignContent: _alignContent.default,
    flex: _flex.default,
    flexGrow: _flexGrow.default,
    flexShrink: _flexShrink.default,
    order: _order.default,
    float: _float.default,
    clear: _clear.default,
    fontFamily: _fontFamily.default,
    fontWeight: _fontWeight.default,
    height: _height.default,
    lineHeight: _lineHeight.default,
    listStylePosition: _listStylePosition.default,
    listStyleType: _listStyleType.default,
    margin: _margin.default,
    maxHeight: _maxHeight.default,
    maxWidth: _maxWidth.default,
    minHeight: _minHeight.default,
    minWidth: _minWidth.default,
    objectFit: _objectFit.default,
    objectPosition: _objectPosition.default,
    opacity: _opacity.default,
    outline: _outline.default,
    overflow: _overflow.default,
    padding: _padding.default,
    placeholderColor: _placeholderColor.default,
    pointerEvents: _pointerEvents.default,
    position: _position.default,
    inset: _inset.default,
    resize: _resize.default,
    boxShadow: _boxShadow.default,
    fill: _fill.default,
    stroke: _stroke.default,
    strokeWidth: _strokeWidth.default,
    tableLayout: _tableLayout.default,
    textAlign: _textAlign.default,
    textColor: _textColor.default,
    fontSize: _fontSize.default,
    fontStyle: _fontStyle.default,
    textTransform: _textTransform.default,
    textDecoration: _textDecoration.default,
    fontSmoothing: _fontSmoothing.default,
    letterSpacing: _letterSpacing.default,
    userSelect: _userSelect.default,
    verticalAlign: _verticalAlign.default,
    visibility: _visibility.default,
    whitespace: _whitespace.default,
    wordBreak: _wordBreak.default,
    width: _width.default,
    zIndex: _zIndex.default,
    gap: _gap.default,
    gridAutoFlow: _gridAutoFlow.default,
    gridTemplateColumns: _gridTemplateColumns.default,
    gridColumn: _gridColumn.default,
    gridColumnStart: _gridColumnStart.default,
    gridColumnEnd: _gridColumnEnd.default,
    gridTemplateRows: _gridTemplateRows.default,
    gridRow: _gridRow.default,
    gridRowStart: _gridRowStart.default,
    gridRowEnd: _gridRowEnd.default,
    transform: _transform.default,
    transformOrigin: _transformOrigin.default,
    scale: _scale.default,
    rotate: _rotate.default,
    translate: _translate.default,
    skew: _skew.default,
    transitionProperty: _transitionProperty.default,
    transitionTimingFunction: _transitionTimingFunction.default,
    transitionDuration: _transitionDuration.default
  });
}