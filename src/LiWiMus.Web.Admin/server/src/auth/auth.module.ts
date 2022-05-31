import { forwardRef, Module } from '@nestjs/common';
import { UsersModule } from '../users/users.module';
import { JwtModule } from '@nestjs/jwt';

@Module({
    imports: [
        forwardRef(() => UsersModule),
        JwtModule.register({
            secret: Buffer.from(process.env.SECRET, 'base64'),
            signOptions: {
                expiresIn: '1h',
            }
        }),
    ],
    exports: [JwtModule],
})
export class AuthModule {}