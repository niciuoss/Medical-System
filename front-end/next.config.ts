/** @type {import('next').NextConfig} */
const nextConfig = {
  // Configurações gerais
  typescript: {
    ignoreBuildErrors: false,
  },
  eslint: {
    ignoreDuringBuilds: false,
  },
  
  // Otimizações para desenvolvimento
  experimental: {
    // Turbopack é habilitado via --turbo flag, não aqui
    optimizePackageImports: ['lucide-react', '@radix-ui/react-icons'],
  },
  
  // Configurações para o sistema médico
  images: {
    domains: ['localhost'],
    formats: ['image/webp', 'image/avif'],
  },
  
  // Headers de segurança
  async headers() {
    return [
      {
        source: '/(.*)',
        headers: [
          {
            key: 'X-Frame-Options',
            value: 'DENY',
          },
          {
            key: 'X-Content-Type-Options',
            value: 'nosniff',
          },
          {
            key: 'Referrer-Policy',
            value: 'origin-when-cross-origin',
          },
        ],
      },
    ]
  },
  
  // Redirects para URLs antigas
  async redirects() {
    return [
      {
        source: '/login',
        destination: '/auth/login',
        permanent: true,
      },
    ]
  },
}

module.exports = nextConfig