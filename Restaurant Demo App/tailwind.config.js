module.exports = {
    purge: {
        enabled: true,
        content: ['./Views/**/*.cshtml']
    },
  darkMode: false, // or 'media' or 'class'
  theme: {
    screens: {
      'xs': '0px',
      'sm': '450px',
      'md': '768px',
      'lg': '1024px'
      }
  },
  variants: {
    extend: {},
  },
  plugins: [],
}
