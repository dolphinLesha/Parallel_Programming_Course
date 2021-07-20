#include <iostream>
#include <ctime>
#include <vector>

using namespace std;

int am_elements=50000;

void print_ar(int*);
void generate_ar(int*);
void posl_sort(int*);
void par_sort(int*);
void posl_sort_batch(int*);

void main()
{
	int * ar = new int[am_elements];
	unsigned int t1;
	unsigned int t2;

	generate_ar(ar);
	//print_ar(ar);
	t1 = clock();
	posl_sort(ar);
	t2 = clock();
	//print_ar(ar);
	cout << "seconds posl: " << (t2 - t1)/1000.0 << endl;

	generate_ar(ar);
	//print_ar(ar);
	t1 = clock();
	par_sort(ar);
	t2 = clock();
	//print_ar(ar);
	cout << "seconds par: " << (t2 - t1) / 1000.0 << endl;

	//generate_ar(ar);
	////print_ar(ar);
	//t1 = clock();
	//posl_sort_batch(ar);
	//t2 = clock();
	////print_ar(ar);
	//cout << "seconds posl2: " << (t2 - t1) / 1000.0 << endl;


	delete[] ar;
	cin.get(); cin.get();
	
}

void generate_ar(int * ar)
{
	srand(time(0));
	for (int i = 0; i < /*sizeof(ar) / sizeof(int)*/am_elements; i++)
	{
		ar[i] = 1 + rand() % 10000;
	}
	
}

void print_ar(int *ar)
{
	cout << endl;
	
	for (int i = 0; i < /*sizeof(ar) / sizeof(int)*/am_elements; i++)
	{
		cout << i << " " << ar[i] << endl;
	}
	
}

void posl_sort(int* ar)
{
	int * ar2 = new int[am_elements];
	for (int i = 0; i < am_elements; i++)
	{
		int x = 0;
		for (int e = 0; e < am_elements; e++)
		{
			if (ar[e] < ar[i] || (ar[i] == ar[e] && e > i))
			{
				x++;
			}
		}
		ar2[x] = ar[i];
	}
	int* p1 = ar;
	int* p2 = ar2;
	for (int i = 0; i < am_elements; i++)
	{
		*(p1 + i) = *(p2 + i);
	}
	delete[] ar2;
}

void par_sort(int* ar)
{
	int* ar2 = new int[am_elements];
	#pragma omp parallel shared(ar2) 
	{
		#pragma omp for schedule(static)
			for (int i = 0; i < am_elements; i++)
			{
				int x = 0;
				for (int e = 0; e < am_elements; e++)
				{
					if (ar[e] < ar[i] || (ar[i] == ar[e] && e > i))
					{
						x++;
					}
				}
				ar2[x] = ar[i];
			}
	}
	
	int* p1 = ar;
	int* p2 = ar2;
	#pragma omp parallel for schedule(static)
	
		for (int i = 0; i < am_elements; i++)
		{
			*(p1 + i) = *(p2 + i);
		}
	
	
	delete[] ar2;
}

int compare(const void* x1, const void* x2)  
{
	return (*(int*)x1 - *(int*)x2);
}

void posl_sort_batch(int* ar)
{
	int batches = 16;
	int sample_s = (int)(am_elements / batches);
	int b2 = 16;
	int s2 = (int)(sample_s / (b2));
	int* arr = new int[batches * (b2-1)];
	int** indexesf = new int*[batches-1];
	int** indexess = new int* [batches - 1];
	int s3 = (int)((batches * (b2 - 1)) / batches);
	for (int i = 0; i < batches; i++)
	{
		indexesf[i] = ar + sample_s*i;
		//cout << endl << i << " " << *indexesf[i] << " ";
		qsort(indexesf[i], sample_s, sizeof(int), compare);
		
	}
	//cout << "\ntrace\n\n";
	for (int i = 0; i < batches; i++)
	{
		for (int e = 0; e < sample_s; e++)
		{
			//cout << i << " " << *(indexesf[i] + e) << endl;
		}
		//cout << "--\n";
		for (int e = 0; e < b2-1; e++)
		{
			arr[i * (b2-1) + e] = *(indexesf[i] + s2*e);
			//cout << i * (b2-1)+e << " " << arr[i * (b2-1)+e] << endl;
		}
	}
	qsort(arr, batches * (b2 - 1), sizeof(int), compare);
	//cout << "\ntrace2\n\n";
	for (int i = 0; i < batches;i++ )
	{
		indexess[i] = arr + s3 * (i+1);
		//cout << i << " " <<  *indexess[i] << endl;
	}
	//cout << "\ntrace4\n\n";
	vector<int> *sorts2 = new vector<int>[batches];
	for (int i = 0; i < batches; i++)
	{
		for (int e = 0; e < am_elements; e++)
		{
			if (i>0 && i <batches-1 && ar[e] < *indexess[i] && ar[e] >= *indexess[i-1])
			{
				sorts2[i].push_back(ar[e]);
				//cout << ar[e] << " ";
			}
			else if (i == 0 && ar[e] < *indexess[i])
			{
				sorts2[i].push_back(ar[e]);
				//cout << ar[e] << " ";
			}
			else if (i == batches-1 && ar[e] >= *indexess[i - 1])
			{
				sorts2[i].push_back(ar[e]);
				//cout << ar[e] << " ";
			}
		}
		//cout << endl << i << "d\n";
	}
	//cout << "\ntrace3\n\n";
	int *ar2 = new int[am_elements];
	int k = 0;
	for (int i = 0; i < batches; i++)
	{
		int m = sorts2[i].size();
		int * p = &ar[k];
		for(int e = 0;e<m;e++)
		{
			ar[k++] = sorts2[i][e];
			//cout << ar[k] << " " ;
		}
		qsort(p, m, sizeof(int), compare);
	}
	
}
 