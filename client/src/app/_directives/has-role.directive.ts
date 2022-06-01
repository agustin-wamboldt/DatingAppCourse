import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs/operators';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appHasRole]' // *appHasRole='["Admin"]'
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[];
  user: User;

  constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>,
    private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
        this.user = user;
      })
  }
  
  ngOnInit(): void {
    // Clear view if no roles.
    if (!this.user?.roles || this.user == null) {
      this.viewContainerRef.clear();
      return;
    }
    
    if (this.user?.roles.some(role => this.appHasRole.includes(role))) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    } // Reads: if the user has one of the roles requested, then we will create this embedded view and use that as our template reference
    else {
      this.viewContainerRef.clear();
    }
  }

}
