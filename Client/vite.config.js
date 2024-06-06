import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react';
import dotenv from 'dotenv'
import path from 'path'

export default defineConfig(() => {
  const envPath = path.resolve(__dirname, '..', '.env');
  dotenv.config({ path: envPath });
  const { LOCALSERVERURL } = process.env;

  return {
    plugins: [react()],
    server: {
      proxy: {
        '/api': {
          target: LOCALSERVERURL,
          changeOrigin: true,
        },
      },
    },
  }
});