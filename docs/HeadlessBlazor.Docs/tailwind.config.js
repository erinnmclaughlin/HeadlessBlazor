/** @type {import('tailwindcss').Config} */

const plugin = require('tailwindcss/plugin')

module.exports = {
  content: ['../../**/*.{html,cshtml,razor,cs}'],
  theme: {
    extend: {},
  },
    plugins: [

        require('@tailwindcss/typography')
    ],
}

