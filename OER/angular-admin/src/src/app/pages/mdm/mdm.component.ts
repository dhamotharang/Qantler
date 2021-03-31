import {HttpEventType, HttpRequest, HttpResponse} from '@angular/common/http';
import {Component, OnInit, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {environment} from '../../../environments/environment';
import {MdmServiceService} from '../../services/mdm/mdm-service.service';
import {ProfileService} from '../../services/profile.service';
import {StorageUploadService} from '../../services/storage-upload.service';
import { Paginator } from 'primeng/components/paginator/paginator';
import _ from 'lodash'

@Component({
  selector: 'app-mdm',
  templateUrl: './mdm.component.html',
  styleUrls: ['./mdm.component.css']
})
export class MdmComponent implements OnInit {
  categories: any[];
  users: any;
  catList: any;
  categoryList: any;
  catEnd: any;
  length: any;
  catStart: number;
  cat_display: boolean;
  catItem: any;
  catForm: FormGroup;
  user: any;
  type: any;
  imgURL: any;
  defaultImage: String = 'assets/images/default-user.png';
  courseImageUploadPercentage: number;
  thumbnailImageUploadStatus: string;
  StorageUrl: any;
  uploadError: boolean;
  weightMaxValue: number=1000000; 
  weightDuplicate: boolean = false;
  weightEntryCheck: boolean = false;
  WeightEditValue : any;
  searchKeyword  : string = "";
  sortType: string = 'desc';
  sortField : number = 0;
  Clickfield :string = '';
  spanColNo : any;
  searchbackup : any;

  @ViewChild('pp') paginator: Paginator;

  constructor(private  mdmService: MdmServiceService,
              private profileService: ProfileService,
              private coolDialogs: ConfirmationService,
              private spinner: NgxSpinnerService,
              protected uploadService: StorageUploadService,
              private messageService: MessageService) {
  }

  ngOnInit() {
    this.catForm = new FormGroup({
      Name: new FormControl(null, [Validators.required, Validators.maxLength(100)]),
      name_Ar: new FormControl(null, [Validators.required, Validators.maxLength(200)]),
      description_Ar: new FormControl(null, Validators.maxLength(10000)),
      description: new FormControl(null, Validators.maxLength(10000)),
      createdBy: new FormControl(null),
      categoryId: new FormControl(null),
      UpdatedBy: new FormControl(null),
      media: new FormControl(null),
      protected: new FormControl(null, Validators.required),
      isResourceProtect: new FormControl(null, Validators.required),
      id: new FormControl(null),
      active: new FormControl(true, Validators.required),
      weight: new FormControl(null, [Validators.required, Validators.max(this.weightMaxValue)]),
    });
    this.user = this.profileService.user;
    this.profileService.getUserDataUpdate().subscribe(() => {
      this.user = this.profileService.user;
    });
    this.uploadError = false;
    this.type = 'Category';
    this.categories = [];
    this.catEnd = 0;
    this.catStart = 0;
    this.length = 0;
    this.catList = [];
    this.cat_display = false;
    this.catItem = null;
    this.getCategories();
    this.courseImageUploadPercentage = 0;
    this.categoryList = [];
  }

  getCategories() {
    this.categories = [];
    this.catEnd = 0;
    this.catStart = 0;
    this.length = 0;
    this.catList = [];
    this.mdmService.getAllCategories().subscribe((response: any) => {
      if (response.hasSucceeded) {
        this.categoryList = response.returnedObject;
      }
    });
    if (this.type === 'Category') {
      this.mdmService.getAllCategories().subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.categories = response.returnedObject;
          this.searchbackup = response.returnedObject;
          this.length = this.categories.length;
          this.catEnd = this.categories.length;
          this.showCat(this.categories);
        } else {
          this.messageService.add({severity: 'error', summary: response.message});
        }
      });
    } else if (this.type === 'Sub Category') {
      this.mdmService.getAllSubCategories().subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.categories = response.returnedObject;
          this.searchbackup = response.returnedObject;
          this.length = this.categories.length;
          this.catEnd = this.categories.length;
          this.showCat(this.categories);
        } else {
          this.messageService.add({severity: 'error', summary: response.message});
        }
      });
    } else if (this.type === 'Education') {
      this.mdmService.getAllEducation().subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.categories = response.returnedObject;
          this.searchbackup = response.returnedObject;
          this.length = this.categories.length;
          this.catEnd = this.categories.length;
          this.showCat(this.categories);
        } else {
          this.messageService.add({severity: 'error', summary: response.message});
        }
      });
    } else if (this.type === 'Institution') {
      this.mdmService.getAllGrades().subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.categories = response.returnedObject;
          this.searchbackup = response.returnedObject;
          this.length = this.categories.length;
          this.catEnd = this.categories.length;
          this.showCat(this.categories);
        } else {
          this.messageService.add({severity: 'error', summary: response.message});
        }
      });
    } else if (this.type === 'Stream') {
      this.mdmService.getAllStreams().subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.categories = response.returnedObject;
          this.searchbackup = response.returnedObject;
          this.length = this.categories.length;
          this.catEnd = this.categories.length;
          this.showCat(this.categories);
        } else {
          this.messageService.add({severity: 'error', summary: response.message});
        }
      });
    } else if (this.type === 'Professions') {
      this.mdmService.getAllLanguages().subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.categories = response.returnedObject;
          this.searchbackup = response.returnedObject;
          this.length = this.categories.length;
          this.catEnd = this.categories.length;
          this.showCat(this.categories);
        } else {
          this.messageService.add({severity: 'error', summary: response.message});
        }
      });
    } else if (this.type === 'Copyright') {
      this.mdmService.getAllCopyrights().subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.categories = response.returnedObject;
          this.searchbackup = response.returnedObject;
          this.length = this.categories.length;
          this.catEnd = this.categories.length;
          this.showCat(this.categories);
        } else {
          this.messageService.add({severity: 'error', summary: response.message});
        }
      });
    } else if (this.type === 'Materials') {
      this.mdmService.getAllQRC().subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.categories = response.returnedObject;
          this.searchbackup = response.returnedObject;
          this.length = this.categories.length;
          this.catEnd = this.categories.length;
          this.showCat(this.categories);
        } else {
          this.messageService.add({severity: 'error', summary: response.message});
        }
      });
    } else if (this.type === 'EducationalStandard') {
      this.mdmService.getAllEducationalStandard().subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.categories = response.returnedObject;
          this.searchbackup = response.returnedObject;
          this.length = this.categories.length;
          this.catEnd = this.categories.length;
          this.showCat(this.categories);
        } else {
          this.messageService.add({severity: 'error', summary: response.message});
        }
      });
    } else if (this.type === 'EducationalUse') {
      this.mdmService.getAllEducationalUse().subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.categories = response.returnedObject;
          this.searchbackup = response.returnedObject;
          this.length = this.categories.length;
          this.catEnd = this.categories.length;
          this.showCat(this.categories);
        } else {
          this.messageService.add({severity: 'error', summary: response.message});
        }
      });
    } else if (this.type === 'EducationLevel') {
      this.mdmService.getAllEducationLevel().subscribe((response: any) => {
        if (response.hasSucceeded) {
          this.categories = response.returnedObject;
          this.searchbackup = response.returnedObject;
          this.length = this.categories.length;
          this.catEnd = this.categories.length;
          this.showCat(this.categories);
        } else {
          this.messageService.add({severity: 'error', summary: response.message});
        }
      });
    }

  }

  showCat(catgory, start = 0) {
    
    const end = (this.catEnd - start) > 10 ? 10 : (this.catEnd - start);
    this.catList = [];
    for (let i = 0; i < end; i++) {
      this.catList[i] = catgory[start + i];
    }
  }

  weightDuplicateCheck(weight)
  { 
    if(this.weightEntryCheck) {  
    for (let i = 0; i < this.catList.length; i++) {
      if(this.catList[i].weight == weight.value && this.WeightEditValue != weight.value)
      { 
      this.weightDuplicate = true;
      return true;      
      }
    }
  }
  this.weightDuplicate = false;
    return false;
  }

  search(){
    this.categories = this.searchbackup;
    ((this.categories && this.categories.length) && (this.catList && this.catList.length)) ? this.paginator.changePage(0):null;
    debugger;
    const versiondata = [];
    let type = this.type;
    var userdata = this.categories;
    var s = this.searchKeyword == undefined || this.searchKeyword == null ? "" : this.searchKeyword;
    if(s == "" ) {
      ((this.categories && this.categories.length) && (this.catList && this.catList.length)) ? this.paginator.changePage(0):null;
      this.searchKeyword = "";
      this.Clickfield = "";
      this.getCategories();
    } else {
      userdata &&  userdata.length
      ?  userdata.map(function mm(i, index) {
         if(i.weight.toString().includes(s.toLowerCase())) {
          versiondata.push(i);
         }
         else if ("Active".toLowerCase().includes(s.toLowerCase())) {
          if(i.active) {
            versiondata.push(i);
          }
         }
         else if ("Inactive".toLowerCase().includes(s.toLowerCase())) {
          if(!i.active) {
            versiondata.push(i);
          }
         }
         else if(i.createdBy.toLowerCase().includes(s.toLowerCase())) {
          versiondata.push(i);
         }

       else if(type != 'EducationalStandard' && type!='Copyright' && type!='EducationLevel') {
          if (i.name.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if(i.name_Ar.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if(type === 'Sub Category' && i.categoryName.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
        }
        else if(type === 'Copyright') {
          if (i.title.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           else if(i.title_Ar.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
          else if(i.description.toLowerCase().includes(s.toLowerCase())) {
          versiondata.push(i);
           }
          else if(i.description_Ar.toLowerCase().includes(s.toLowerCase())) {
          versiondata.push(i);
           }
          else if(i.protected.toString().includes(s.toLowerCase().toString())) {
          versiondata.push(i);
           }
          else if(i.isResourceProtect.toString().includes(s.toLowerCase().toString())) {
          versiondata.push(i);
           }
      }
        else if(type === 'EducationalStandard') {
          if (i.standard.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
           if (i.standard_Ar.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
        } 
        else if(type === 'EducationLevel') {
        if (i.level.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
        else if (i.level_Ar.toLowerCase().includes(s.toLowerCase())) {
            versiondata.push(i);
           }
          }
        }) : [];
        userdata = versiondata;
        this.length = userdata.length;
        this.catEnd= userdata.length;
        this.categories = userdata;
        this.showCat(userdata);
    }
  }

  Clickingevent(event,columnNo) {
    debugger;
    var type = this.type;
    var userdata = this.categories;
    if(this.Clickfield === event.target.id) {
      this.sortType = this.sortType === 'desc' ? 'asc' : 'desc';
    } else {
      this.Clickfield = event.target.id
      this.sortType = 'asc'
    }
    this.sortField = columnNo;
    ((this.categories && this.categories.length) && (this.catList && this.catList.length)) ? this.paginator.changePage(0):null;
    this.spanColNo = this.sortType+"-"+columnNo;
    var sortedArray = _.sortBy(userdata, function(patient) {
      if(event.target.id === 'title'){
        if(type === 'Copyright') {
          return patient.title;
        }
        else if(type !='EducationalStandard' && type !='Copyright' && type !='EducationLevel' ) {
          return patient.name;
        }
        else if(type ==='EducationalStandard') {
          return patient.standard;
        }
        else if(type ==='EducationLevel') {
          return patient.level;
        }
      }
      else if(event.target.id === 'artitle'){
        if(type === 'Copyright') {
          return patient.title_Ar;
        }
        else if(type !='EducationalStandard' && type !='Copyright' && type !='EducationLevel' ) {
          return patient.name_Ar;
        }
        else if(type ==='EducationalStandard') {
          return patient.standard_Ar;
        }
        else if(type ==='EducationLevel') {
          return patient.level_Ar;
        }
      } 
      else if(event.target.id === 'category'){
        return patient.categoryName;
      }
      else if(event.target.id === 'desc'){
        return patient.description;
      }
      else if(event.target.id === 'ardesc'){
        return patient.description_Ar;
      }
      else if(event.target.id === 'remix'){
        return patient.protected;
      }
      else if(event.target.id === 'res'){
        return patient.isResourceProtect;
      }
      else if(event.target.id === 'weight'){
        return patient.weight;
      }
      else if(event.target.id === 'status'){
        return patient.active;
      }
      else if(event.target.id === 'createdby'){
        return patient.createdBy;
      }
  });
  if(this.sortType == 'desc') {
    sortedArray =   sortedArray.reverse();
  }
    this.length = sortedArray.length;
    this.categories = sortedArray;
    this.catEnd= userdata.length;
    this.showCat(this.categories);
    debugger;
    }

  clearSearch(){
    ((this.categories && this.categories.length) && (this.catList && this.catList.length)) ? this.paginator.changePage(0):null;
    this.searchKeyword = "";
    this.Clickfield = "";
    this.getCategories();
  }

  paginateCat(event) {
    this.showCat(this.categories, event.first);
  }

  addNew() {
    this.WeightEditValue=null;
    this.cat_display = true;
    this.catItem = null;
    this.catForm.reset();
    if (this.type === 'Category') {
      this.catForm.patchValue({
        weight:0,
        protected: false,
        isResourceProtect: false,
        active: true
      });
    } else if (this.type === 'Sub Category') {
      this.catForm.patchValue({
        weight:0,
        protected: true,
        isResourceProtect: false,
        active: true
      });
    } else if (this.type === 'Education') {
      this.catForm.patchValue({
        weight:0,
        protected: true,
        isResourceProtect: false,
        active: true
      });
    } else if (this.type === 'Institution') {
      this.catForm.patchValue({
        weight:0,
        protected: true,
        isResourceProtect: false,
        active: true
      });
    } else if (this.type === 'Stream') {
      this.catForm.patchValue({
        weight:0,
        protected: false,
        isResourceProtect: false,
        active: true
      });
    } else if (this.type === 'Professions') {
      this.catForm.patchValue({
        weight:0,
        protected: false,
        isResourceProtect: false,
        active: true
      });
    } else if (this.type === 'Materials') {
      this.catForm.patchValue({
        weight:0,
        protected: false,
        isResourceProtect: false,
        active: true
      });
    } else if (this.type === 'EducationalStandard') {
      this.catForm.patchValue({
        weight:0,
        protected: false,
        isResourceProtect: false,
        active: true
      });
    } else if (this.type === 'EducationalUse') {
      this.catForm.patchValue({
        weight:0,
        protected: false,
        isResourceProtect: false,
        active: true
      });
    } else if (this.type === 'EducationLevel') {
      this.catForm.patchValue({
        weight:0,
        protected: false,
        isResourceProtect: false,
        active: true
      });
    } else {
      this.catForm.patchValue({
        weight:0,
        active: true
      });
    }
  }

  showCatDialog(item) {
    this.user = this.profileService.user;
    this.WeightEditValue=item.weight;
    if (item.id && this.user) {
      this.cat_display = true;
      this.catItem = item;
      if (this.type === 'Sub Category') {
        this.catForm.patchValue({
          Name: item.name ? item.name : item.title,
          name_Ar: item.name_Ar ? item.name_Ar : item.title_Ar,
          UpdatedBy: this.user.id,
          categoryId: item.categoryId,
          active: item.active,
          protected: false,
          isResourceProtect: false,
          weight: item.weight
        });
      } else if (this.type === 'Copyright') {
        this.catForm.patchValue({
          Name: item.name ? item.name : item.title,
          name_Ar: item.name_Ar ? item.name_Ar : item.title_Ar,
          description_Ar: item.description_Ar,
          UpdatedBy: this.user.id,
          description: item.description,
          media: item.media,
          active: item.active,
          protected: item.protected.toString(),
          isResourceProtect: item.isResourceProtect.toString(),
          weight: item.weight
        });
      } else if (this.type === 'EducationalStandard') {
        this.catForm.patchValue({
          Name: item.standard,
          name_Ar: item.standard_Ar,
          UpdatedBy: this.user.id,
          active: item.active,
          protected: false,
          isResourceProtect: false,
          weight: item.weight
        });
      } else if (this.type === 'EducationLevel') {
        this.catForm.patchValue({
          Name: item.level,
          name_Ar: item.level_Ar,
          UpdatedBy: this.user.id,
          active: item.active,
          protected: false,
          isResourceProtect: false,
          weight: item.weight
        });
      } else {
        this.catForm.patchValue({
          Name: item.name ? item.name : item.title,
          name_Ar: item.name_Ar ? item.name_Ar : item.title_Ar,
          UpdatedBy: this.user.id,
          active: item.active,
          protected: false,
          isResourceProtect: false,
          weight: Number(item.weight)
        });
      }
    }


  }

  submitCat(data) {
    this.user = this.profileService.user;
    if (data && this.user) {
      this.catForm.patchValue({
        createdBy: this.user.id,
      });
      this.spinner.show();
      if (this.type === 'Category') {
        this.mdmService.postCategory(this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record added Successfully'});
            this.getCategories();
            this.spinner.hide();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();

            this.spinner.hide();
          }
        });
      } else if (this.type === 'Sub Category') {
        this.mdmService.postSubCategory(this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record added Successfully'});
            this.getCategories();
            this.spinner.hide();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
            this.spinner.hide();
          }
        });
      } else if (this.type === 'Education') {
        this.mdmService.postEducation(this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record added Successfully'});
            this.getCategories();
            this.spinner.hide();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
            this.spinner.hide();
          }
        });
      } else if (this.type === 'Institution') {
        this.mdmService.postGrades(this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record added Successfully'});
            this.getCategories();
            this.spinner.hide();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
            this.spinner.hide();
          }
        });
      } else if (this.type === 'Stream') {
        this.mdmService.postStreams(this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record added Successfully'});
            this.getCategories();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
          }
          this.spinner.hide();
        });
      } else if (this.type === 'Professions') {
        this.mdmService.postLanguages(this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record added Successfully'});
            this.getCategories();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
          }
          this.spinner.hide();
        });
      } else if (this.type === 'Copyright') {
        this.mdmService.postCopyrights(this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record added Successfully'});
            this.getCategories();
            this.spinner.hide();
          } else {
            if (response.message) {
              this.messageService.add({severity: 'error', summary: response.message});
              this.getCategories();
              this.spinner.hide();
            }
          }
        });
      } else if (this.type === 'Materials') {
        this.mdmService.postQRC(this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record added Successfully'});
            this.getCategories();
            this.spinner.hide();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
            this.spinner.hide();
          }
        });
      } else if (this.type === 'EducationalStandard') {
        this.mdmService.postEducationalStandard(this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record added Successfully'});
            this.getCategories();
            this.spinner.hide();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
            this.spinner.hide();
          }
        });
      } else if (this.type === 'EducationalUse') {
        this.mdmService.postEducationalUse(this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record added Successfully'});
            this.getCategories();
            this.spinner.hide();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
            this.spinner.hide();
          }
        });
      } else if (this.type === 'EducationLevel') {
        this.mdmService.postEducationLevel(this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record added Successfully'});
            this.getCategories();
            this.spinner.hide();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
            this.spinner.hide();
          }
        });
      }
    }


  }

  updateCat(data) {
    this.user = this.profileService.user;
    if (data && this.user) {
      this.catForm.patchValue({
        createdBy: this.user.id,
        id: data.id
      });

      this.spinner.show();
      if (this.type === 'Category') {
        this.mdmService.updateCategory(this.catItem.id, this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record updated Successfully'});
            this.getCategories();
            this.spinner.hide();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
            this.spinner.hide();
          }
        });
      } else if (this.type === 'Sub Category') {
        this.mdmService.updateSubCategory(this.catItem.id, this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record updated Successfully'});
            this.getCategories();
            this.spinner.hide();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
            this.spinner.hide();
          }
        });
      } else if (this.type === 'Education') {
        this.mdmService.updateEducation(this.catItem.id, this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record updated Successfully'});
            this.getCategories();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
          }
          this.spinner.hide();
        });
      } else if (this.type === 'Institution') {
        this.mdmService.updateGrades(this.catItem.id, this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record updated Successfully'});
            this.getCategories();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
          }
          this.spinner.hide();
        });
      } else if (this.type === 'Stream') {
        this.mdmService.updateStreams(this.catItem.id, this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record updated Successfully'});
            this.getCategories();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
          }
          this.spinner.hide();
        });
      } else if (this.type === 'Professions') {
        this.mdmService.updateLanguages(this.catItem.id, this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record updated Successfully'});
            this.getCategories();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
          }
          this.spinner.hide();
        });
      } else if (this.type === 'Copyright') {
        this.mdmService.updateCopyrights(this.catItem.id, this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record updated Successfully'});
            this.getCategories();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
          }
          this.spinner.hide();
        });
      } else if (this.type === 'Materials') {
        this.mdmService.updateQRC(this.catItem.id, this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record updated Successfully'});
            this.getCategories();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
          }
          this.spinner.hide();
        });
      } else if (this.type === 'EducationalStandard') {
        this.mdmService.updateEducationalStandard(this.catItem.id, this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record updated Successfully'});
            this.getCategories();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
          }
          this.spinner.hide();
        });
      } else if (this.type === 'EducationalUse') {
        this.mdmService.updateEducationalUse(this.catItem.id, this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record updated Successfully'});
            this.getCategories();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
          }
          this.spinner.hide();
        });
      } else if (this.type === 'EducationLevel') {
        this.mdmService.updateEducationLevel(this.catItem.id, this.catForm.value).subscribe((response: any) => {
          if (response.hasSucceeded) {
            this.catForm.reset();
            this.cat_display = false;
            this.messageService.add({severity: 'success', summary: 'Record updated Successfully'});
            this.getCategories();
          } else {
            this.messageService.add({severity: 'error', summary: response.message});
            this.getCategories();
          }
          this.spinner.hide();
        });
      }
    }


  }

  deleteCat(item) {

    // if(!item.active){
    this.coolDialogs.confirm({
      message: 'Are you sure that you want to remove this?',
      accept: () => {
        this.spinner.show();
        if (this.type === 'Category') {
          this.mdmService.deleteCategory(item.id).subscribe((response: any) => {
            if (response.hasSucceeded) {
              this.catForm.reset();
              this.cat_display = false;
              this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
              this.getCategories();
            } else {
              this.messageService.add({severity: 'error', summary: response.message});
              this.getCategories();
            }
            this.spinner.hide();
          });
        } else if (this.type === 'Sub Category') {
          this.mdmService.deleteSubCategory(item.id).subscribe((response: any) => {
            if (response.hasSucceeded) {
              this.catForm.reset();
              this.cat_display = false;
              this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
              this.getCategories();
            } else {
              this.messageService.add({severity: 'error', summary: response.message});
              this.getCategories();
            }
            this.spinner.hide();
          });
        } else if (this.type === 'Education') {
          this.mdmService.deleteEducation(item.id).subscribe((response: any) => {
            if (response.hasSucceeded) {
              this.catForm.reset();
              this.cat_display = false;
              this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
              this.getCategories();
            } else {
              this.messageService.add({severity: 'error', summary: response.message});
              this.getCategories();
            }
            this.spinner.hide();
          });
        } else if (this.type === 'Institution') {
          this.mdmService.deleteGrades(item.id).subscribe((response: any) => {
            if (response.hasSucceeded) {
              this.catForm.reset();
              this.cat_display = false;
              this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
              this.getCategories();
            } else {
              this.messageService.add({severity: 'error', summary: response.message});
              this.getCategories();
            }
            this.spinner.hide();
          });
        } else if (this.type === 'Stream') {
          this.mdmService.deleteStreams(item.id).subscribe((response: any) => {
            if (response.hasSucceeded) {
              this.catForm.reset();
              this.cat_display = false;
              this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
              this.getCategories();
            } else {
              this.messageService.add({severity: 'error', summary: response.message});
              this.getCategories();
            }
            this.spinner.hide();
          });
        } else if (this.type === 'Professions') {
          this.mdmService.deleteLanguages(item.id).subscribe((response: any) => {
            if (response.hasSucceeded) {
              this.catForm.reset();
              this.cat_display = false;
              this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
              this.getCategories();
            } else {
              this.messageService.add({severity: 'error', summary: response.message});
              this.getCategories();
            }
            this.spinner.hide();
          });
        } else if (this.type === 'Copyright') {
          this.mdmService.deleteCopyrights(item.id).subscribe((response: any) => {
            if (response.hasSucceeded) {
              this.catForm.reset();
              this.cat_display = false;
              this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
              this.getCategories();
            } else {
              this.messageService.add({severity: 'error', summary: response.message});
              this.getCategories();
            }
            this.spinner.hide();
          });
        } else if (this.type === 'Materials') {
          this.mdmService.deleteQRC(item.id).subscribe((response: any) => {
            if (response.hasSucceeded) {
              this.catForm.reset();
              this.cat_display = false;
              this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
              this.getCategories();
            } else {
              this.messageService.add({severity: 'error', summary: response.message});
              this.getCategories();
            }
            this.spinner.hide();
          });
        } else if (this.type === 'EducationalStandard') {
          this.mdmService.deleteEducationalStandard(item.id).subscribe((response: any) => {
            if (response.hasSucceeded) {
              this.catForm.reset();
              this.cat_display = false;
              this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
              this.getCategories();
            } else {
              this.messageService.add({severity: 'error', summary: response.message});
              this.getCategories();
            }
            this.spinner.hide();
          });
        } else if (this.type === 'EducationalUse') {
          this.mdmService.deleteEducationalUse(item.id).subscribe((response: any) => {
            if (response.hasSucceeded) {
              this.catForm.reset();
              this.cat_display = false;
              this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
              this.getCategories();
            } else {
              this.messageService.add({severity: 'error', summary: response.message});
              this.getCategories();
            }
            this.spinner.hide();
          });
        } else if (this.type === 'EducationLevel') {
          this.mdmService.deleteEducationLevel(item.id).subscribe((response: any) => {
            if (response.hasSucceeded) {
              this.catForm.reset();
              this.cat_display = false;
              this.messageService.add({severity: 'success', summary: ' Record Deleted Successfully'});
              this.getCategories();
            } else {
              this.messageService.add({severity: 'error', summary: response.message});
              this.getCategories();
            }
            this.spinner.hide();
          });
        }
      }
    });
    // }else{
    //   this.messageService.add({severity: 'error', summary: 'Please update as inative record'});
    // }

  }

  updateOptions(event) {
    this.type = event.target.value;
    this.getCategories();
  }

  handleFileInput(files: FileList) {
    console.log(files[0]);
    if (files[0]) {
      const type = files[0].name.split('.')[files[0].name.split('.').length - 1];
      if (type.toLowerCase() === 'jpg' || type.toLowerCase() === 'jpeg' || type.toLowerCase() === 'png' || type.toLowerCase() === 'bmp') {
        this.uploadError = false;
        const reader = new FileReader();
        reader.readAsDataURL(files[0]);
        reader.onload = () => {
          this.imgURL = reader.result;
          if (typeof reader.result === 'string') {
            this.thumbnailImageUploadStatus = 'Uploading';
            const splitFile = files[0].name.split('.');
            const data = [{
              fileName: this.uploadService.copyrightFolder + Date.now() +
              this.profileService.makeid(10) + '.' + splitFile[splitFile.length - 1],
              fileBase64: reader.result.split(',')[1]
            }];
            this.courseImageUploadPercentage = 0;
            this.uploadService.upload(new HttpRequest('POST', environment.apiUrl + 'ContentMedia/UploadFiles', data,
              {
                reportProgress: true
              })).subscribe(event => {
              if (event.type === HttpEventType.UploadProgress) {
                this.courseImageUploadPercentage = Math.round(100 * event.loaded / event.total);
              } else if (event instanceof HttpResponse) {
                if (event.body.hasSucceed) {
                  this.thumbnailImageUploadStatus = 'Uploaded';
                  this.messageService.add({
                    severity: 'success',
                    summary: 'copyright media Uploaded',
                    key: 'toast',
                    life: 5000
                  });
                  this.catForm.patchValue({
                    media: event.body.fileUrl
                  });
                } else {
                  this.messageService.add({
                    severity: 'error',
                    summary: 'Failed to upload copyright media',
                    key: 'toast',
                    life: 5000
                  });
                  this.thumbnailImageUploadStatus = 'Failed';
                }
              }
            });
          }
        };
      } else {
        this.uploadError = true;
        this.messageService.add({severity: 'error', summary: 'Invalid file type'});
      }
    }
  }

  removeFile() {
    this.catForm.patchValue({
      media: null,
    });
  }

  numberOnly(event): boolean {    
    this.weightEntryCheck=true;
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) || (charCode >= 96 && charCode <= 105) ) {
      return false;
    }
    var target = event.target ? event.target : event.srcElement;   
    return true;
  }

}
