import {
    CanActivate,
    ExecutionContext,
    Injectable,
    UnauthorizedException,
    UseGuards,
} from '@nestjs/common';
import {Observable} from 'rxjs';
import {JwtService} from '@nestjs/jwt';

@Injectable()
export class JwtAuthGuard implements CanActivate {
    constructor(private jwtService: JwtService) {
    }

    canActivate(
        ctx: ExecutionContext,
    ): boolean | Promise<boolean> | Observable<boolean> {

        const request = ctx
            .switchToHttp()
            .getRequest();

        const authHeader = request.headers.authorization;

        if (!authHeader) {
            throw new UnauthorizedException({
                message: 'There is no authorization header',
            });
        }
        const [tokenType, token] = authHeader.split(' ');
        
        if (tokenType !== 'Bearer') {
            throw new UnauthorizedException({
                message: `The authorization header ${tokenType} type is not supported`,
            });
        }
        
        if (!token) {
            throw new UnauthorizedException({
                message: `There is no bearer token`,
            });
        }
        
        let user;
        try {
            user = this.jwtService
                .verify(token, { 
                    secret: Buffer.from(process.env.SECRET, 'base64') 
                });
        } catch (e) {
            throw new UnauthorizedException()
        }


        if (!user) {
            throw new UnauthorizedException({
                message: 'Authorization token is not valid',
            });
        }
        
        if (user.sysperm.every(x => x !== 'Admin.Access')) {
            throw new UnauthorizedException({
                message: 'You don\'t have access',
            });
        }
        
        return true;
    }
}

export const AuthorizeAdmin = () => UseGuards(JwtAuthGuard);