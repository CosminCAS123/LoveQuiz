import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';
import os  from 'node:os';
import tailwindcss from '@tailwindcss/vite' //david imported this, it's used under 'plugins'

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "lovequiz.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
//     if (0 !== child_process.spawnSync('dotnet', [
//         'dev-certs',
//         'https',
//         '--export-path',
//         certFilePath,
//         '--format',
//         'Pem',
//         '--no-password',
//     ], { stdio: 'inherit', }).status) {
//         throw new Error("Could not create certificate.");
//     }
// }

const dotnetAvailable = (() => {
    try {
        child_process.execSync('dotnet --version', { stdio: 'ignore' });
        return true;
    } catch {
        return false;
    }
})();

if (dotnetAvailable && (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath))) {
    child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit' });
}

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7279';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin(), tailwindcss()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '^/api': {
                target,
                secure: false
            }
        },
        port: 5173,
        // https: {
        //     key: fs.readFileSync(keyFilePath),
        //     cert: fs.readFileSync(certFilePath),
        // }
        https: fs.existsSync(certFilePath) && fs.existsSync(keyFilePath)
            ? {
                key: fs.readFileSync(keyFilePath),
                cert: fs.readFileSync(certFilePath),
            }
            : false
    }
})
