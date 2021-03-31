import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Router} from '@angular/router';
import {ResourceService} from '../../services/resource.service';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-categories-modal',
  templateUrl: './categories-modal.component.html'
})
export class CategoriesModalComponent implements OnInit {
  @Input() display: boolean;
  @Output() displayChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  categories: any;
  subCategories: any;
  selectedSubCategories: any;

  constructor(private resourceService: ResourceService, private router: Router, private translate: TranslateService) {
  }

  ngOnInit() {
    this.display = false;
    this.categories = [];
    this.subCategories = [];
    this.selectedSubCategories = [];
    this.getCategories();
  }

  getCurrentLang() {
    return this.translate.currentLang;
  }

  sortArray(array, string) {
    return array.sort((n1, n2) => {
      if (n1[string] > n2[string]) {
        return 1;
      }

      if (n1[string] < n2[string]) {
        return -1;
      }

      return 0;
    });
  }

  getCategories() {
    this.resourceService.getResourceMasterData().subscribe((res) => {
      if (res.hasSucceeded) {
        this.categories = res.returnedObject.categoryMasterData;
        this.subCategories =res.returnedObject.subCategoryMasterData;
      }
    });
  }

  closeModel() {
    this.display = false;
    this.selectedSubCategories = [];
    this.displayChange.emit(false);
  }

  submitModal() {
    if (this.router.url.split('/').find(x => x === 'course')) {
      const filter = {
        'ca': [],
        'sc': this.selectedSubCategories ? this.selectedSubCategories : [],
        'es': [],
        'eu': [],
        'le': [],
        'mt': [],
        'co': [],
        'pr': [],
        'ed': [],
        'p': 0,
        's': ''
      };
      this.router.navigate(['/courses', {q: '', f: JSON.stringify(filter)}]);
    }
    if (this.router.url.split('/').find(x => x === 'resource')) {
      const filter = {
        'ca': [],
        'sc': this.selectedSubCategories ? this.selectedSubCategories : [],
        'es': [],
        'eu': [],
        'le': [],
        'mt': [],
        'co': [],
        'pr': [],
        'ed': [],
        'p': 0,
        's': ''
      };
      this.router.navigate(['/resources', {q: '', f: JSON.stringify(filter)}]);
    }
  }
}
