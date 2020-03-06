"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.default = _default;

var _lodash = _interopRequireDefault(require("lodash"));

var _flattenColorPalette = _interopRequireDefault(require("../util/flattenColorPalette"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _default() {
  return function ({
    addUtilities,
    e,
    theme,
    variants
  }) {
    const utilities = _lodash.default.fromPairs(_lodash.default.map((0, _flattenColorPalette.default)(theme('fill')), (value, modifier) => {
      return [`.${e(`fill-${modifier}`)}`, {
        fill: value
      }];
    }));

    addUtilities(utilities, variants('fill'));
  };
}