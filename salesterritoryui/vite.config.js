import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import { PORTS } from './src/config/ports.js'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    open: true, // Automatically open browser when dev server starts
    port: PORTS.REACT_PORT
  }
})
